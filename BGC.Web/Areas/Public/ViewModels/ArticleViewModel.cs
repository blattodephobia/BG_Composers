﻿using BGC.Web.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BGC.Web.Areas.Public.ViewModels
{
    public class ArticleViewModel : ViewModelBase
    {
        public string Text { get; set; }

        public string Title { get; set; }

        public ImageViewModel ProfilePicture { get; set; }
    }
}