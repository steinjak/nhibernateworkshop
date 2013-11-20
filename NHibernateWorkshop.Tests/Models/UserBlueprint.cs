using System;
using NHibernateWorkshop.Models;
using Plant.Core;

namespace NHibernateWorkshop.Tests.Models
{
    public class UserBlueprint : IBlueprint
    {
        private readonly string[] firstNames = new[]
        {
            "Mohamed", "Youssef", "Ahmed", "Mahmoud", "Mustafa", "Yassin", "Taha", "Khaled", "Hamza", "Bilal", "Ibrahim", "Hassan", "Hussein", "Karim", "Tareq", "Abdel-Rahman", "Ali", "Omar", "Halim", "Murad", "Selim", "Abdallah",
            "Shaimaa", "Fatma", "Maha", "Reem", "Farida", "Aya", "Shahd", "Ashraqat", "Sahar", "Fatin", "Dalal", "Doha", "Fajr", "Suha", "Rowan", "Hosniya", "Hasnaa", "Hosna", "Gamila", "Gamalat", "Habiba",
            "Alba", "Alda", "Amalia", "Amanda", "Angela", "Arina", "Bjørg", "Brá", "Durita", "Emilia", "Eydna", "Guðrun", "Hervør", "Inga", "Ingibjørg", "Jóna", "Kira", "Liljan", "Linda", "Lýdia", "Malan", "Marin", "Marita", "Marjun", "Nadja", "Natalia", "Rannvá", "Rita", "Ró", "Ronja", "Súsanna", "Una",
            "Bárður", "Bjarti", "Brynjar", "Dávur", "Elias", "Fríði", "Hákun", "Ísakur", "Jan", "Jógvan", "Jóhan", "Jósef", "Kári", "Magnus", "Markus", "Martin", "Niklas", "Pætur", "Rasmus", "Sjúrður", "Villiam"
        };

        private readonly string[] lastNames = new[]
        {
            "Armijo", "Armstead", "Armstrong", "Arndt", "Arnett", "Arnold", "Arredondo", "Arreola", "Arriaga", "Arrington", "Arroyo", "Arsenault", "Arteaga", "Arthur", "Artis", "Asbury", "Ash", "Ashby", "Ashcraft", "Ashe",
            "Nesbitt", "Nesmith", "Ness", "Nettles", "Neuman", "Neumann", "Nevarez", "Neville", "New", "Newberry", "Newby", "Newcomb", "Newell", "Newkirk", "Newman", "Newsom", "Newsome", "Newton", "Ng", "Ngo", "Nguyen", "Nicholas", "Nichols", "Nicholson", "Nickel", "Nickerson", "Nielsen",
            "Rendon", "Renfro", "Renner", "Reno", "Renteria", "Reyes", "Reyna", "Reynolds", "Reynoso", "Rhea", "Rhoades", "Rhoads", "Rhodes", "Ricci", "Rice", "Rich", "Richard", "Richards", "Richardson", "Richey", "Richie", "Richmond", "Richter", "Rickard", "Ricker", "Ricketts", "Rickman", "Ricks", "Rico",
            "Varela", "Vargas", "Varner", "Varney", "Vasquez", "Vaughan", "Vaughn", "Vaught", "Vazquez", "Veal", "Vega", "Vela", "Velasco", "Velasquez", "Velazquez", "Velez", "Venable", "Venegas", "Ventura", "Vera", "Verdin", "Vernon", "Vest", "Vetter"
        };

        private readonly Random random = new Random();

        public void SetupPlant(BasePlant p)
        {
            p.DefinePropertiesOf<User>(new
            {
                Username = new Sequence<string>(i => "user-" + i),
                Name = new Sequence<string>(i => string.Format("{0} {1}", firstNames[random.Next(firstNames.Length)], lastNames[random.Next(lastNames.Length)]))
            });
        }
    }
}