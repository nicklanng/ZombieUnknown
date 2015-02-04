﻿using System.Collections.Generic;
using System.Linq;
using Engine.Drawing;
using Engine.Entities;
using Engine.Pathfinding;
using Engine.Sprites;
using Microsoft.Xna.Framework;

namespace Engine.Maps
{
    public class PathfindingMap : IDrawingProvider
    {
        private Node[,] _nodes;
        private List<Sprite>[,] _tileSprites; 

        public int Height { get; private set; }
        public int Width { get; private set; }

        public PathfindingMap()
        {
            var map = GameState.Map;
            Height = map.Height;
            Width = map.Width;
            RegeneratePathfindingMap();
        }

        public Node GetNodeAt(Coordinate coordinate)
        {
            return _nodes[coordinate.X, coordinate.Y];
        }
        
        public IEnumerable<DrawingRequest> GetDrawings()
        {
            var drawingRequests = new List<DrawingRequest>();

            for (var y = 0; y < Height; y++)
            {
                for (var x = 0; x < Width; x++)
                {
                    var tileIcons = _tileSprites[x, y];
                    var mapPosition = new Vector2(x, y);

                    drawingRequests.AddRange(tileIcons.Select(tileIcon => new DrawingRequest(tileIcon, mapPosition, Color.DarkRed)));
                }
            }

            return drawingRequests;
        }


        private void RegeneratePathfindingMap()
        {
            var map = GameState.Map;
            _tileSprites = new List<Sprite>[map.Width, map.Height];
            _nodes = new Node[map.Width, map.Height];
            for (var x = 0; x < map.Width; x++)
            {
                for (var y = 0; y < map.Height; y++)
                {
                    var node = new Node(new Coordinate(x, y));
                    _nodes[x, y] = node;

                    _tileSprites[x, y] = new List<Sprite>(8);
                }
            }

            for (var x = 0; x < map.Width; x++)
            {
                for (var y = 0; y < map.Height; y++)
                {
                    var node = _nodes[x, y];

                    var thisCoord = new Coordinate(x, y);
                    var thisTile = map.GetTile(thisCoord);

                    var upCoord = thisCoord + Coordinate.NorthWest;

                    var upLeftCoord = thisCoord + Coordinate.West;
                    var upLeftTile = map.GetTile(upLeftCoord);

                    var leftCoord = thisCoord + Coordinate.SouthWest;
                    var leftTile = map.GetTile(leftCoord);

                    var downLeftCoord = thisCoord + Coordinate.South;
                    var downLeftTile = map.GetTile(downLeftCoord);

                    var downCoord = thisCoord + Coordinate.SouthEast;
                    var downTile = map.GetTile(downCoord);

                    var downRightCoord = thisCoord + Coordinate.East;
                    var downRightTile = map.GetTile(downRightCoord);

                    var rightCoord = thisCoord + Coordinate.NorthEast;
                    var rightTile = map.GetTile(rightCoord);

                    var upRightCoord = thisCoord + Coordinate.North;
                    var upRightTile = map.GetTile(upRightCoord);

                    // up
                    if (map.IsPositionOnMap(upCoord))
                    {
                        if (upLeftTile == null || upRightTile == null)
                        {
                            continue;
                        }

                        if (!thisTile.HasLeftWall && !thisTile.HasRightWall && !upLeftTile.HasRightWall && !upRightTile.HasLeftWall)
                        {
                            node.AddNeighbor(_nodes[upCoord.X, upCoord.Y]);
                        }
                    }

                    // up left
                    if (map.IsPositionOnMap(upLeftCoord))
                    {
                        if (!thisTile.HasLeftWall)
                        {
                            node.AddNeighbor(_nodes[upLeftCoord.X, upLeftCoord.Y]);
                        }
                    }

                    // left
                    if (map.IsPositionOnMap(leftCoord))
                    {
                        if (leftTile == null || downLeftTile == null)
                        {
                            continue;
                        }

                        if (!thisTile.HasLeftWall && !leftTile.HasRightWall && !downLeftTile.HasLeftWall && !downLeftTile.HasRightWall)
                        {
                            node.AddNeighbor(_nodes[leftCoord.X, leftCoord.Y]);
                        }
                    }

                    // down left
                    if (map.IsPositionOnMap(downLeftCoord))
                    {
                        if (!downLeftTile.HasRightWall)
                        {
                            node.AddNeighbor(_nodes[downLeftCoord.X, downLeftCoord.Y]);
                        }
                    }

                    // down
                    if (map.IsPositionOnMap(downCoord))
                    {
                        if (downTile == null || downLeftTile == null || downRightTile == null)
                        {
                            continue;
                        }

                        if (!downTile.HasLeftWall && !downTile.HasRightWall && !downLeftTile.HasRightWall && !downRightTile.HasLeftWall)
                        {
                            node.AddNeighbor(_nodes[downCoord.X, downCoord.Y]);
                        }
                    }

                    // down right
                    if (map.IsPositionOnMap(downRightCoord))
                    {
                        if (!downRightTile.HasLeftWall)
                        {
                            node.AddNeighbor(_nodes[downRightCoord.X, downRightCoord.Y]);
                        }
                    }

                    // right
                    if (map.IsPositionOnMap(rightCoord))
                    {
                        if (rightTile == null || downRightTile == null)
                        {
                            continue;
                        }

                        if (!thisTile.HasRightWall && !rightTile.HasLeftWall && !downRightTile.HasLeftWall && !downRightTile.HasRightWall)
                        {
                            node.AddNeighbor(_nodes[rightCoord.X, rightCoord.Y]);
                        }
                    }

                    // up right
                    if (map.IsPositionOnMap(upRightCoord))
                    {
                        if (!thisTile.HasRightWall)
                        {
                            node.AddNeighbor(_nodes[upRightCoord.X, upRightCoord.Y]);
                        }
                    }
                }
            }
        }

        public void AddBlockage(PhysicalEntity blockage)
        {
            var position = (Coordinate) blockage.MapPosition;
            var thisNode = GetNodeAt(position);

            var upCoord = position + Coordinate.NorthWest;
            var upTile = GetNodeAt(upCoord);
            upTile.Neighbors.Remove(thisNode);

            var upLeftCoord = position + Coordinate.West;
            var upLeftTile = GetNodeAt(upLeftCoord);
            upLeftTile.Neighbors.Remove(thisNode);

            var leftCoord = position + Coordinate.SouthWest;
            var leftTile = GetNodeAt(leftCoord);
            leftTile.Neighbors.Remove(thisNode);

            var downLeftCoord = position + Coordinate.South;
            var downLeftTile = GetNodeAt(downLeftCoord);
            downLeftTile.Neighbors.Remove(thisNode);

            var downCoord = position + Coordinate.SouthEast;
            var downTile = GetNodeAt(downCoord);
            downTile.Neighbors.Remove(thisNode);

            var downRightCoord = position + Coordinate.East;
            var downRightTile = GetNodeAt(downRightCoord);
            downRightTile.Neighbors.Remove(thisNode);

            var rightCoord = position + Coordinate.NorthEast;
            var rightTile = GetNodeAt(rightCoord);
            rightTile.Neighbors.Remove(thisNode);

            var upRightCoord = position + Coordinate.North;
            var upRightTile = GetNodeAt(upRightCoord);
            upRightTile.Neighbors.Remove(thisNode);

            //_nodes[position.X, position.Y] = new Node(position);
        }
    }
}