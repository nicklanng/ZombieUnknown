using System;
using System.Collections.Generic;
using System.Linq;
using ClipperLib;
using Engine.Drawing;
using Engine.Entities;
using Engine.Extensions;
using Engine.Maths;
using Engine.Pathfinding;
using Engine.Sprites;
using Microsoft.Xna.Framework;

namespace Engine.Maps
{
    public class PathfindingMap : IDrawingProvider
    {
        private Node[,] _nodes;
        private List<Sprite>[,] _tileSprites;
        public List<List<IntPoint>> NavMesh { get; private set; }
        public List<List<IntPoint>> NoGoAreas { get; private set; }

        public int Height { get; private set; }
        public int Width { get; private set; }

        public PathfindingMap()
        {
            NavMesh = new List<List<IntPoint>>(); 

            var map = GameState.Map;
            Height = map.Height;
            Width = map.Width;
            RegeneratePathfindingMap();

            var floorClipper = new Clipper();
            var wallClipper = new Clipper();
            for (var y = 0; y < Height; y++)
            {
                for (var x = 0; x < Width; x++)
                {
                    var tile = map.GetTile(new Coordinate(x, y));

                    if (tile.HasFloor)
                    {
                        var floorPolygon = new List<IntPoint>
                        {
                            new IntPoint(10*x - 1, 10*y - 1),
                            new IntPoint(10*x + 11, 10*y - 1),
                            new IntPoint(10*x + 11, 10*y + 11),
                            new IntPoint(10*x - 1, 10*y + 11)
                        };
                        floorClipper.AddPath(floorPolygon, PolyType.ptSubject, Closed: true);
                    }

                    if (tile.HasLeftWall)
                    {
                        var wallPolygon = new List<IntPoint>
                        {
                            new IntPoint(10*x - 4, 10*y - 4),
                            new IntPoint(10*x + 14, 10*y - 4),
                            new IntPoint(10*x + 14, 10*y + 4),
                            new IntPoint(10*x - 4, 10*y + 4)
                        };
                        wallClipper.AddPath(wallPolygon, PolyType.ptSubject, Closed: true);
                    }

                    if (tile.HasRightWall)
                    {
                        var wallPolygon = new List<IntPoint>
                        {
                            new IntPoint(10*x - 4, 10*y - 4),
                            new IntPoint(10*x + 4, 10*y - 4),
                            new IntPoint(10*x + 4, 10*y + 14),
                            new IntPoint(10*x - 4, 10*y + 14)
                        };
                        wallClipper.AddPath(wallPolygon, PolyType.ptSubject, Closed: true);
                    }
                }
            }

            var floors = new List<List<IntPoint>>();
            floorClipper.Execute(ClipType.ctUnion, floors, PolyFillType.pftNonZero, PolyFillType.pftNonZero);
            
            //join walls
            NoGoAreas = new List<List<IntPoint>>();
            wallClipper.Execute(ClipType.ctUnion, NoGoAreas, PolyFillType.pftNonZero, PolyFillType.pftNonZero);

            //subtract walls
            var clipper = new Clipper();
            clipper.AddPaths(floors, PolyType.ptSubject, closed: true);
            clipper.AddPaths(NoGoAreas, PolyType.ptClip, closed: true);
            clipper.Execute(ClipType.ctDifference, NavMesh);

            /*
            var floorTriPolys = floors.Select(polygon => new Polygon(polygon.Select(point => new PolygonPoint(point.X, point.Y)))).ToList();
            var floor = floorTriPolys.First();
            var wallTriPolys = walls.Select(polygon => new Polygon(polygon.Select(point => new PolygonPoint(point.X, point.Y)).Reverse())).ToList();
            foreach (var wallTriPoly in wallTriPolys)
            {
                floor.AddHole(wallTriPoly);
            }
            for (var y = 2.5; y < Height; y+=5)
            {
                for (var x = 2.5; x < Width; x+=5)
                {
                    //var steinerPoint = new TriangulationPoint(x*10, y*10);
                    //if (wallTriPolys.Any(t => t.IsPointInside(steinerPoint))) continue;
                    //floor.Add(steinerPoint);
                }
            }
            Poly2Tri.P2T.Triangulate(floor);

            
            System.Console.WriteLine("X,Y");
            foreach (var triangle in floor.Triangles)
            {
                for (var i = 0; i < triangle.Points.Count(); i++)
                {
                    System.Console.Write(triangle.Points[i].X + " " + triangle.Points[i].Y + " 0 ");
                }
                System.Console.WriteLine();
            }
            */
             
        }

        public Node GetNodeAt(Coordinate coordinate)
        {
            return _nodes[coordinate.X, coordinate.Y];
        }

        public bool DirectPathBetween(Vector2 origin, Vector2 target)
        {
            var checkLine = new Line(origin, target);

            foreach (var area in NavMesh)
            {
                for (var i = 0; i < area.Count; i++)
                {
                    var j = (i + 1) % area.Count;
                    var wall = new Line(new Vector2(area[i].X / 10.0f, area[i].Y / 10.0f), new Vector2(area[j].X / 10.0f, area[j].Y / 10.0f));
                    float howFarThrough;
                    if (checkLine.IntersectsLine(wall, out howFarThrough))
                    {
                        var angle = Math.Acos(Vector2.Dot((target-origin).NormalizeFixed(), wall.Normal.NormalizeFixed()));
                        if (angle < MathHelper.PiOver2)
                        {
                            continue;
                        }
                        return false;
                    }
                }
            }

            return true;
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
            var blocker = blockage as IMovementBlocker;
            if (blocker == null) return;

            var position = (Coordinate)blockage.MapPosition;
            var thisNode = GetNodeAt(position);

            var upCoord = position + Coordinate.NorthWest;
            var upTile = GetNodeAt(upCoord);

            var upLeftCoord = position + Coordinate.West;
            var upLeftTile = GetNodeAt(upLeftCoord);

            var leftCoord = position + Coordinate.SouthWest;
            var leftTile = GetNodeAt(leftCoord);

            var downLeftCoord = position + Coordinate.South;
            var downLeftTile = GetNodeAt(downLeftCoord);

            var downCoord = position + Coordinate.SouthEast;
            var downTile = GetNodeAt(downCoord);

            var downRightCoord = position + Coordinate.East;
            var downRightTile = GetNodeAt(downRightCoord);

            var rightCoord = position + Coordinate.NorthEast;
            var rightTile = GetNodeAt(rightCoord);

            var upRightCoord = position + Coordinate.North;
            var upRightTile = GetNodeAt(upRightCoord);

            if (blocker.BlocksTile)
            {
                upRightTile.Neighbors.Remove(thisNode);
                upTile.Neighbors.Remove(thisNode);
                upLeftTile.Neighbors.Remove(thisNode);
                leftTile.Neighbors.Remove(thisNode);
                downLeftTile.Neighbors.Remove(thisNode);
                downTile.Neighbors.Remove(thisNode);
                downRightTile.Neighbors.Remove(thisNode);
                rightTile.Neighbors.Remove(thisNode);
            }

            if (blocker.BlocksDiagonals)
            {
                downLeftTile.Neighbors.Remove(upLeftTile);
                downLeftTile.Neighbors.Remove(downRightTile);

                upLeftTile.Neighbors.Remove(upRightTile);
                upLeftTile.Neighbors.Remove(downLeftTile);

                upRightTile.Neighbors.Remove(upLeftTile);
                upRightTile.Neighbors.Remove(downRightTile);

                downRightTile.Neighbors.Remove(upRightTile);
                downRightTile.Neighbors.Remove(downLeftTile);
            }


            //_nodes[position.X, position.Y] = new Node(position);
        }

        public bool IsPointOutOfBounds(Vector2 mapPosition)
        {
            var xCoord = (int) Math.Round(mapPosition.X*10);
            var yCoord = (int) Math.Round(mapPosition.Y*10);

            return NoGoAreas.Select(area => Clipper.PointInPolygon(new IntPoint(xCoord, yCoord), area) != 0).Any(inArea => inArea);
        }

        public IEnumerable<Line> GetWallsInsideRectangle(Rectangle rectangle)
        {
            var navmesh = NavMesh;
            foreach (var area in navmesh)
            {
                for (var i = 0; i < area.Count; i++)
                {
                    var j = (i + 1)%area.Count;
                    var wall = new Line(new Vector2(area[i].X / 10.0f, area[i].Y / 10.0f), new Vector2(area[j].X / 10.0f, area[j].Y / 10.0f));

                    if (wall.IntersectsRect(rectangle))
                    {
                        yield return wall;
                    }
                }
            }
        }
    }
}