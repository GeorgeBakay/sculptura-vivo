using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UBox.Date.Models;

namespace UBox.Date
{
    public class AppDBContext: DbContext
    {
        public AppDBContext(DbContextOptions<AppDBContext> options) : base(options)
        {
            
        }
        protected override void OnModelCreating(ModelBuilder modelbuilder)
        {
            base.OnModelCreating(modelbuilder);

            //Налаштування зв'язки в таблиці UserDetailInfo містить всі звязки для User
            modelbuilder.Entity<UserDetailInfo>().HasMany(u => u.follower).WithOne(e => e.FollowerUser).HasForeignKey(o => o.FollowerUserId).OnDelete(DeleteBehavior.Cascade);
            modelbuilder.Entity<UserDetailInfo>().HasMany(u => u.following).WithOne(e => e.FollowingUser).HasForeignKey(o => o.FollowingUserId).OnDelete(DeleteBehavior.Cascade);
            modelbuilder.Entity<UserDetailInfo>().HasMany(u => u.likes).WithOne(e => e.User).HasForeignKey(o => o.UserId).OnDelete(DeleteBehavior.Cascade);
            modelbuilder.Entity<UserDetailInfo>().HasOne(u => u.avatar).WithOne(e => e.User).HasForeignKey<UserAvatarImage>(o => o.UserId).OnDelete(DeleteBehavior.Cascade);

            //Налаштування зв'язки в таблиці FollowArray містить хто і на кого підписаний
            modelbuilder.Entity<FollowArray>().HasOne<UserDetailInfo>(e => e.FollowerUser).WithMany(u => u.follower).OnDelete(DeleteBehavior.ClientSetNull);
            modelbuilder.Entity<FollowArray>().HasOne<UserDetailInfo>(e => e.FollowingUser).WithMany(u => u.following).OnDelete(DeleteBehavior.ClientSetNull);

            //Налаштування зв'язки таблиц Post таблиці яка зберігає пости викладени користувачем
            modelbuilder.Entity<Post>().HasMany(u => u.Likes).WithOne(e => e.Post).OnDelete(DeleteBehavior.ClientSetNull);

            //Налаштування зв'язки таблиц Like таблиці яка зберігає лайки зроблені користувачем
            modelbuilder.Entity<Like>().HasOne(u => u.Post).WithMany(e => e.Likes).OnDelete(DeleteBehavior.ClientSetNull);
            modelbuilder.Entity<Like>().HasOne(u => u.User).WithMany(e => e.likes).OnDelete(DeleteBehavior.ClientSetNull);

            //Налаштування зв'язки в таблиці UserAvatarImage де зберігаються аватарки користувачів
            modelbuilder.Entity<UserAvatarImage>().HasOne(u => u.User).WithOne(e => e.avatar).OnDelete(DeleteBehavior.ClientSetNull);
        }
        public DbSet<User> Users { get; set; }
        public DbSet<UserAvatarImage> AvatarImages{ get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<FollowArray> FollowArrays { get; set; }
        public DbSet<UserDetailInfo> UserDetailInfos { get; set; }
        public DbSet<Like> Likes { get; set; }
    }
}
