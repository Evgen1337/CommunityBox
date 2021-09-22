﻿// <auto-generated />
using System;
using CommunityBox.ChatService.DAL;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace CommunityBox.ChatService.DAL.Migrations
{
    [DbContext(typeof(ChatContext))]
    partial class ChatContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 63)
                .HasAnnotation("ProductVersion", "5.0.9")
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            modelBuilder.Entity("CommunityBox.ChatService.Domain.Entities.Chat", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.HasKey("Id");

                    b.ToTable("Chats");
                });

            modelBuilder.Entity("CommunityBox.ChatService.Domain.Entities.ChatUser", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<long>("ChatId")
                        .HasColumnType("bigint");

                    b.Property<string>("UserId")
                        .HasColumnType("text");

                    b.Property<string>("UserName")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("ChatId");

                    b.HasIndex("UserId")
                        .HasDatabaseName("ChatUser_UserIdIndex");

                    b.ToTable("ChatUsers");
                });

            modelBuilder.Entity("CommunityBox.ChatService.Domain.Entities.Message", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<long>("ChatId")
                        .HasColumnType("bigint");

                    b.Property<string>("Content")
                        .HasColumnType("text");

                    b.Property<DateTime>("ReceivedDateUtc")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("UserId")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("ChatId");

                    b.HasIndex("UserId")
                        .HasDatabaseName("Message_UserIdIndex");

                    b.ToTable("Messages");
                });

            modelBuilder.Entity("CommunityBox.ChatService.Domain.Entities.ChatUser", b =>
                {
                    b.HasOne("CommunityBox.ChatService.Domain.Entities.Chat", "Chat")
                        .WithMany("ChatUsers")
                        .HasForeignKey("ChatId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Chat");
                });

            modelBuilder.Entity("CommunityBox.ChatService.Domain.Entities.Message", b =>
                {
                    b.HasOne("CommunityBox.ChatService.Domain.Entities.Chat", "Chat")
                        .WithMany("Messages")
                        .HasForeignKey("ChatId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Chat");
                });

            modelBuilder.Entity("CommunityBox.ChatService.Domain.Entities.Chat", b =>
                {
                    b.Navigation("ChatUsers");

                    b.Navigation("Messages");
                });
#pragma warning restore 612, 618
        }
    }
}
