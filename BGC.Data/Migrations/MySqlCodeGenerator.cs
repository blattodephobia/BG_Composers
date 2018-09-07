using BGC.Utilities;
using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations.Design;
using System.Data.Entity.Migrations.Model;
using System.Data.Entity.Migrations.Utilities;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BGC.Data.Migrations
{
    internal class MySqlCodeGenerator : CSharpMigrationCodeGenerator
    {
        private static string StripDbo(string identifier)
        {
            if (identifier.StartsWith("dbo."))
            {
                return identifier.Substring(4);
            }
            else
            {
                return identifier;
            }
        }

        // this is a workaround for MySQL .NET connector's bug #74726 (EF migrations fail on long foreign keys)
        private static void SanitizeConstraintName(ForeignKeyOperation op)
        {
            Func<string, bool> IsTooLong = (s) => s?.Length >= 64;
            if (op.HasDefaultName || IsTooLong(op.Name))
            {
                string principal = op.PrincipalTable.GetHashCode().ToString("X");
                string dependant = StripDbo(op.DependentTable);
                string columns = op.DependentColumns.ToStringAggregate("+");
                op.Name = $"FK_{principal}:{dependant}.{columns}";

                if (IsTooLong(op.Name))
                {
                    dependant = dependant.GetHashCode().ToString("X");
                    columns = columns.GetHashCode().ToString("X");
                    op.Name = $"FK_{principal}:{dependant}.{columns}";
                }
            }
        }

        protected override string Quote(string identifier)
        {
            /* Sigh... yet another workaround for MySQL's .NET Connector functionality.
             * Quote(string) is called by EntityFramework for wrapping ALL identifiers, variables and others when
             * migrations are generated. We can use this opportunity to check for any and all strings starting with
             * "dbo." and sanitize them. It's not pretty, but the other approach would be to override all Generate()
             * methods. And not all of their parameters can have the values written to the migration changed so that
             * "dbo." is no longer present (e.g. DropTableOperation - its Name property is read-only).
             * */
            return base.Quote(StripDbo(identifier));
        }
        
        protected override void Generate(AddForeignKeyOperation addForeignKeyOperation, IndentedTextWriter writer)
        {
            SanitizeConstraintName(addForeignKeyOperation);
            base.Generate(addForeignKeyOperation, writer);
        }

        protected override void Generate(DropForeignKeyOperation dropForeignKeyOperation, IndentedTextWriter writer)
        {
            SanitizeConstraintName(dropForeignKeyOperation);
            base.Generate(dropForeignKeyOperation, writer);
        }

        protected override void GenerateInline(AddForeignKeyOperation addForeignKeyOperation, IndentedTextWriter writer)
        {
            SanitizeConstraintName(addForeignKeyOperation);

            writer.WriteLine();
            writer.Write(".ForeignKey(" + Quote(StripDbo(addForeignKeyOperation.PrincipalTable)) + ", ");
            Generate(addForeignKeyOperation.DependentColumns, writer);

            if (addForeignKeyOperation.CascadeDelete)
            {
                writer.Write(", cascadeDelete: true");
            }

            if (!string.IsNullOrEmpty(addForeignKeyOperation.Name))
            {
                writer.Write($", name: {Quote(addForeignKeyOperation.Name)}");
            }

            writer.Write(")");
        }
    }
}
