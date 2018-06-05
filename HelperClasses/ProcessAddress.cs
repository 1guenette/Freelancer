using System;
using System.Collections.Generic;


namespace MG_5_FreelanceJobsite.HelperClasses
{
    public class ProcessAddress
    {
        public Dictionary<decimal, decimal> latlong(string address)
        {
            var latLong = new Dictionary<decimal, decimal>();

            const string url = "https://maps.googleapis.com/maps/api/geocode/json?key=AIzaSyD1hogFVpNDlrNOeg1V5z4PB4NSTpCJ5Tw&sensor=true&address=";

            dynamic googleResults = new Uri(url + address).GetDynamicJsonObject();

            foreach (var result in googleResults.results)
            {
                //Have to do a specific cast or we'll get a C# runtime binding exception
                var lat = (decimal)result.geometry.location.lat;
                var lng = (decimal)result.geometry.location.lng;

                latLong.Add(lat, lng);
            }

            return latLong;
        }

        public double calcualteDistanceCord(double lat1, double lon1, double lat2, double lon2, char unit)
        {
            double theta = lon1 - lon2;
            double dist = Math.Sin(deg2rad(lat1)) * Math.Sin(deg2rad(lat2)) + Math.Cos(deg2rad(lat1)) * Math.Cos(deg2rad(lat2)) * Math.Cos(deg2rad(theta));
            dist = Math.Acos(dist);
            dist = rad2deg(dist);
            dist = dist * 60 * 1.1515;

            if (unit == 'K')
            {
                dist = dist * 1.609344;
            }
            else if (unit == 'N')
            {
                dist = dist * 0.8684;
            }

            return (dist);
        }

        private double deg2rad(double deg)
        {
            return (deg * Math.PI / 180.0);
        }
        private double rad2deg(double rad) { return (rad / Math.PI * 180.0); }



    }

}