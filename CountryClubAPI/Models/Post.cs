//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace CountryClubAPI.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Post
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Post()
        {
            this.Post_has_Comment = new HashSet<Post_has_Comment>();
            this.PostLikedbyUsers = new HashSet<PostLikedbyUser>();
        }
    
        public int Post_ID { get; set; }
        public string message { get; set; }
        public Nullable<int> Likes { get; set; }
        public byte[] Image { get; set; }
        public string TimePosted { get; set; }
        public string TimeUpdated { get; set; }
        public int User_ID { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Post_has_Comment> Post_has_Comment { get; set; }
        public virtual User User { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PostLikedbyUser> PostLikedbyUsers { get; set; }
    }
}
