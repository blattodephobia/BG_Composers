﻿using BGC.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BGC.Data.Relational
{
    [Table(nameof(ComposerArticle) + "_" + nameof(MediaTypeInfo))]
    internal class ArticleMedia : RelationdalDtoBase
    {
        public ArticleRelationalDto Article { get; set; }

        public MediaTypeInfoRelationalDto MediaEntry { get; set; }

        [Key, Column(Order = 0)]
        [ForeignKey(nameof(Article))]
        public int Article_Id { get; set; }

        [Key, Column(Order = 1)]
        [ForeignKey(nameof(MediaEntry))]
        public int MediaEntry_Id { get; set; }
    }
}
