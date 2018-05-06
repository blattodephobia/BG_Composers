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

        protected override void Generate(CreateTableOperation createTableOperation, IndentedTextWriter writer)
        {
            base.Generate(createTableOperation, writer);
        }
    }
}
