using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Chat.Models.DataBase
{
    public class ApplicationContext : DbContext
    {
        public DbSet<UserModel> Users { get; set; }
        public DbSet<MessageModel> ChatMessages { get; set; }
        public DbSet<RoomModel> ChatRooms { get; set; }
        public DbSet<RoomMemberModel> RoomMembers { get; set; }

        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)
        {

        }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserModel>();
            modelBuilder.Entity<MessageModel>();
            modelBuilder.Entity<RoomModel>();
            modelBuilder.Entity<RoomMemberModel>();
        }

    }
}
