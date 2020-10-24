using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;

namespace Engine.Models
{
    public class World
    {
        private List<Location> _locations = new List<Location>();

        internal void AddLocation(Location location) => _locations.Add(location);

        public Location LocationAt(int xCoordinate, int yCoordinate)
        {
            try { return _locations.First(l => (l.XCoordinate == xCoordinate && l.YCoordinate == yCoordinate)); } catch { return null; };
        }

        public List<Location> GetAllLocations()
        {
            List<Location> visitedLocations = new List<Location>();

            foreach (Location location in _locations)
            {
                if (location.PlayerHasVisitedHere) visitedLocations.Add(location);
            }

            return visitedLocations;
        }

        //internal void AddLocation(int xCoordinate, int yCoordinate, string name, string description, string ImageFileName, bool playerHasvisitedHere = false)
        //{
        //    if (xCoordinate == 0 && yCoordinate == 0) playerHasvisitedHere = true;

        //    _locations.Add(new Location(xCoordinate,yCoordinate,name,description,Paths.Images.Locations(ImageFileName),playerHasvisitedHere));
        //            }

    }
}
