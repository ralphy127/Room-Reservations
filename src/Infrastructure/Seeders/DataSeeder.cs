using System.Collections.Generic;
using MongoDB.Driver;
using RoomRes.Domain.Models;

namespace RoomRes.Infrastructure.Seeders {
    public class DataSeeder {
        private readonly IMongoDatabase _database;

        public DataSeeder(IMongoDatabase database) {
            _database = database;
        }

        public async Task SeedAsync() {
            IMongoCollection<Room> roomsCollection = _database.GetCollection<Room>("rooms");
            IMongoCollection<User> usersCollection = _database.GetCollection<User>("users");
            IMongoCollection<RoomReservation> reservationsCollection = _database.GetCollection<RoomReservation>("room_reservations");

            if (!await roomsCollection.Find(_ => true).AnyAsync()) {
                List<Room> rooms = new List<Room> {
                    new Room("1", 20),
                    new Room("2", 20),
                    new Room("3", 40),
                    new Room("4", 60),
                    new Room("11", 15),
                    new Room("12", 15),
                    new Room("13", 20),
                    new Room("14", 20),
                    new Room("15", 10),
                    new Room("16", 10),
                    new Room("21", 10),
                    new Room("22", 15),
                    new Room("23", 20)
                };

                await roomsCollection.InsertManyAsync(rooms);
            }

            if (!await usersCollection.Find(_ => true).AnyAsync()) {
                List<User> users = new List<User> {
                    new User("1", "admin", "admin"),
                    new User("2", "Pablo", "blahblah"),
                    new User("3", "Andrew", "password321"),
                    new User("4", "Alex", "dog33"),
                    new User("5", "Sophia", "strongpassword"),
                    new User("6", "Michael", "michael123"),
                    new User("7", "Rachel", "rachelpass"),
                    new User("8", "John", "johnpassword"),
                    new User("9", "Olivia", "olivia2023"),
                    new User("10", "Ethan", "ethan1234")
                    };

                await usersCollection.InsertManyAsync(users);
            }

            if (!await reservationsCollection.Find(_ => true).AnyAsync()) {
                List<RoomReservation> reservations = new List<RoomReservation> {
                    new RoomReservation("1", "2", "2", DateTime.Now.AddDays(-1.5), DateTime.Now.AddDays(2.2), TimeSpan.FromHours(2)),
                    new RoomReservation("2", "14", "3", DateTime.Now.AddDays(-1), DateTime.Now.AddDays(1), TimeSpan.FromHours(1.5)),
                    new RoomReservation("3", "15", "1", DateTime.Now.AddDays(-0.5), DateTime.Now.AddDays(1.5), TimeSpan.FromHours(2.5)),
                    new RoomReservation("4", "4", "4", DateTime.Now, DateTime.Now.AddDays(2.5), TimeSpan.FromHours(2.25)),
                    new RoomReservation("5", "13", "5", DateTime.Now.AddDays(1), DateTime.Now.AddDays(1).AddHours(1), TimeSpan.FromHours(1.5)),
                    new RoomReservation("6", "3", "6", DateTime.Now.AddDays(2), DateTime.Now.AddDays(2).AddHours(2), TimeSpan.FromHours(2)),
                    new RoomReservation("7", "12", "7", DateTime.Now.AddHours(-2), DateTime.Now.AddHours(1), TimeSpan.FromHours(2.5)),
                    new RoomReservation("8", "5", "8", DateTime.Now.AddDays(3), DateTime.Now.AddDays(3).AddHours(0.5), TimeSpan.FromHours(1)),
                    new RoomReservation("9", "8", "9", DateTime.Now.AddDays(4), DateTime.Now.AddDays(4).AddHours(2), TimeSpan.FromHours(2.5)),
                    new RoomReservation("10", "16", "10", DateTime.Now.AddDays(2).AddHours(2), DateTime.Now.AddDays(2).AddHours(4), TimeSpan.FromHours(2)),
                    new RoomReservation("11", "14", "6", DateTime.Now.AddDays(1), DateTime.Now.AddDays(1).AddHours(3), TimeSpan.FromHours(3)),
                    new RoomReservation("12", "7", "4", DateTime.Now.AddDays(1).AddHours(-3), DateTime.Now.AddDays(1).AddHours(1), TimeSpan.FromHours(1.5)),
                    new RoomReservation("13", "2", "3", DateTime.Now.AddDays(5), DateTime.Now.AddDays(5).AddHours(1), TimeSpan.FromHours(2)),
                    new RoomReservation("14", "9", "2", DateTime.Now.AddDays(6), DateTime.Now.AddDays(6).AddHours(0.5), TimeSpan.FromHours(1)),
                    new RoomReservation("15", "11", "8", DateTime.Now.AddDays(1), DateTime.Now.AddDays(1).AddHours(2), TimeSpan.FromHours(1.5))
                };

                await reservationsCollection.InsertManyAsync(reservations);
            }
        }
    }
}
