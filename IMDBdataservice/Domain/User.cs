using System;
using System.Collections.Generic;

#nullable disable

namespace IMDBdataservice
{
    public partial class User
    {
        public User()
        {
            Comments = new HashSet<Comment>();
            SearchHistories = new HashSet<SearchHistory>();
        }

        public long UserId { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Salt { get; set; }
        public DateTime CreatedDate { get; set; }

        public virtual ICollection<Comment> Comments { get; set; }
        public virtual ICollection<SearchHistory> SearchHistories { get; set; }
        public virtual ICollection<BookmarkTitle> BookmarkTitles{ get; set; }
    }
}
