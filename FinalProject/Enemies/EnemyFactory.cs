using System;
using System.Collections.Generic;

namespace FinalProject.Classes
{
    public static class EnemyFactory
    {
        private static Random rng = new Random();

        public static List<Enemy> CreateTribbleEncounter()
        {
            List<Enemy> list = new List<Enemy>();

            // Spawn 1–3 tribbles initially
            int count = rng.Next(1, 4);

            for (int i = 0; i < count; i++)
                list.Add(new Tribble());

            return list;
        }
    }
}
