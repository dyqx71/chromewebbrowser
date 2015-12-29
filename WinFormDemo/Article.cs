using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WinFormDemo
{
   public  class Article
    {

       public Article()
       {
           this.title = "";
           this.content = "";
           this.thumbnail = "";
           this.tags = "";
           this.article_like = "0";
           this.dislike = "0";
           this.like = "0";
           this.viewCount = "0";

       }

        private string title;

        public string Title
        {
            get { return title; }
            set { title = value; }
        }

        private string url;

        public string Url
        {
            get { return url; }
            set { url = value; }
        }

        private List<Images> images;

        public List<Images> Images
        {
            get { return images; }
            set { images = value; }
        }
        private string thumbnail;

        public string Thumbnail
        {
            get { return thumbnail; }
            set { thumbnail = value; }
        }
        private string category;

        public string Category
        {
            get { return category; }
            set { category = value; }
        }
        private string tags;

        public string Tags
        {
            get { return tags; }
            set { tags = value; }
        }
        private string content;

        public string Content
        {
            get { return content; }
            set { content = value; }
        }

        private string viewCount;

        public string ViewCount
        {
            get { return viewCount; }
            set { viewCount = value; }
        }

        private string article_like;

        public string Article_like
        {
            get { return article_like; }
            set { article_like = value; }
        }

        private string like;

        public string Like
        {
            get { return like; }
            set { like = value; }
        }

        private string dislike;

        public string Dislike
        {
            get { return dislike; }
            set { dislike = value; }
        }

        public override string ToString()
        {
            return "article_like:" + article_like + ",like:" + like + ",dislike:" + dislike + ",category:" + category;
        }
    }
}
