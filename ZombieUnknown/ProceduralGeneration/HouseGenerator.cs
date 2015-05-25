using System;
using System.Collections.Generic;
using System.Linq;
using Engine.Extensions;
using Microsoft.Xna.Framework;

namespace ZombieUnknown.ProceduralGeneration
{
    class HouseGenerator
    {
        public Building GenerateHouse(int width, int height)
        {
            var roomsToPlace = ChooseRoomsToPlace(width, height);

            var emptySpace = new Rectangle(0, 0, width, height);
            InflateRoomArea(roomsToPlace, emptySpace.GetArea());

            var placedRooms = new List<Room>(roomsToPlace.Count);
            while (roomsToPlace.Any())
            {
                var rectangles = PlaceVertically(emptySpace.Width, emptySpace.Height)
                    ? FitRoomsVertically(ref emptySpace, roomsToPlace)
                    : FitRoomsHoritontally(ref emptySpace, roomsToPlace);
                placedRooms.AddRange(rectangles);
                roomsToPlace = roomsToPlace.Skip(rectangles.Count()).ToList();
            }

            return new Building(new Rectangle(0, 0, width, height), placedRooms);
        }

        private static List<RoomRequirement> ChooseRoomsToPlace(int width, int height)
        {
            return RoomRequirementProvider.Instance.HouseRooms;
        }

        private static void InflateRoomArea(IList<RoomRequirement> roomsToPlace, int area)
        {
            var sum = roomsToPlace.Sum(x => x.Area);
            for (var index = 0; index < roomsToPlace.Count; index++)
            {
                var room = roomsToPlace[index];
                room.Area /= sum;
                room.Area *= area;
                roomsToPlace[index] = room;
            }
        }

        private static bool PlaceVertically(int width, int height)
        {
            return width > height;
        }

        private static List<Room> FitRoomsVertically(ref Rectangle emptySpace, IReadOnlyCollection<RoomRequirement> rooms)
        {
            var oldAspectRatio = float.MaxValue;
            var numberOfRoomsToFit = 1;

            var currentRooms = new List<Room>(numberOfRoomsToFit);

            var blockWidth = 0;

            if (rooms.Count == 1)
            {
                var room = new Room(rooms.First().Type, new Rectangle(emptySpace.X, emptySpace.Y, emptySpace.Width, emptySpace.Height));
                currentRooms.Add(room);
                emptySpace = new Rectangle(0, 0, 0, 0);
                return currentRooms;
            }

            while (numberOfRoomsToFit <= rooms.Count)
            {
                var roomsToFit = rooms.Take(numberOfRoomsToFit).ToList();

                // get width of block of rooms
                var roomsTotalArea = roomsToFit.Sum(x => x.Area);
                var roomWidth = roomsTotalArea / emptySpace.Height;

                // get aspect ratio of smallest room
                var smallRoomHeight = roomsToFit.OrderBy(x => x.Area).First().Area / roomWidth;
                var newAspectRatio = GetAspectRatio(roomWidth, smallRoomHeight);

                if (newAspectRatio < oldAspectRatio)
                {
                    currentRooms = new List<Room>(numberOfRoomsToFit);
                    // build rooms
                    var randomisedRooms = roomsToFit.OrderBy(x => Guid.NewGuid());
                    var xCoord = emptySpace.X;
                    var yCoord = emptySpace.Y;
                    currentRooms.Clear();
                    foreach (var room in randomisedRooms)
                    {
                        blockWidth = (int)Math.Round(roomWidth);
                        var roundedHeight = (int)Math.Round(room.Area / roomWidth);
                        currentRooms.Add(new Room(room.Type, new Rectangle(xCoord, yCoord, blockWidth, roundedHeight)));
                        yCoord += roundedHeight;
                    }

                    oldAspectRatio = newAspectRatio;
                    numberOfRoomsToFit++;
                }
                else
                {
                    break;
                }
            }

            emptySpace = new Rectangle(emptySpace.X + blockWidth, emptySpace.Y, emptySpace.Width - blockWidth, emptySpace.Height);
            return currentRooms;
        }

        private static List<Room> FitRoomsHoritontally(ref Rectangle emptySpace, IReadOnlyCollection<RoomRequirement> rooms)
        {
            var oldAspectRatio = float.MaxValue;
            var numberOfRoomsToFit = 1;

            var currentRooms = new List<Room>(numberOfRoomsToFit);

            var blockHeight = 0;

            if (rooms.Count == 1)
            {
                var room = new Room(rooms.First().Type, new Rectangle(emptySpace.X, emptySpace.Y, emptySpace.Width, emptySpace.Height));
                currentRooms.Add(room);
                emptySpace = new Rectangle(0, 0, 0, 0);
                return currentRooms;
            }

            while (numberOfRoomsToFit <= rooms.Count)
            {
                var roomsToFit = rooms.Take(numberOfRoomsToFit).ToList();

                // get height of block of rooms
                var roomsTotalArea = roomsToFit.Sum(x => x.Area);
                var roomHeight = roomsTotalArea / emptySpace.Width;

                // get aspect ratio of smallest room
                var smallRoomWidth = roomsToFit.OrderBy(x => x.Area).First().Area / roomHeight;
                var newAspectRatio = GetAspectRatio(roomHeight, smallRoomWidth);

                if (newAspectRatio < oldAspectRatio)
                {
                    currentRooms = new List<Room>(numberOfRoomsToFit);
                    // build rooms
                    var randomisedRooms = roomsToFit.OrderBy(x => Guid.NewGuid());
                    var xCoord = emptySpace.X;
                    var yCoord = emptySpace.Y;
                    currentRooms.Clear();
                    foreach (var room in randomisedRooms)
                    {
                        blockHeight = (int)Math.Round(roomHeight);
                        if (numberOfRoomsToFit == rooms.Count)
                        {
                            blockHeight = emptySpace.Height;
                        }
                        var roundedWidth = (int)Math.Round(room.Area / roomHeight);
                        currentRooms.Add(new Room(room.Type, new Rectangle(xCoord, yCoord, roundedWidth, blockHeight)));
                        xCoord += roundedWidth;
                    }

                    oldAspectRatio = newAspectRatio;
                    numberOfRoomsToFit++;
                }
                else
                {
                    break;
                }
            }

            emptySpace = new Rectangle(emptySpace.X, emptySpace.Y + blockHeight, emptySpace.Width, emptySpace.Height - blockHeight);
            return currentRooms;
        }

        private static float GetAspectRatio(float width, float height)
        {
            return Math.Max(width / height, height / width);
        }
    }
}
