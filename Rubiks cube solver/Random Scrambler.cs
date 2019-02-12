using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rubiks_cube_solver_app
{
    class Random_Scrambler
    {
        private string[] turn_combo = { "U ", "U2 ", "U' ",  "F ", "F2 ", "F' ", "L ", "L2 ", "L' ", "R ", "R2 ", "R' ", "D ", "D2 ", "D' ", "B ", "B2 ", "B' " };

        public string Return_Combo()
        {
            Random random = new Random();
            string return_value = null;
            for (int i = 0; i <= 30; i++)
            {
         
                return_value = return_value + turn_combo[random.Next(0, 17)];
            }
            return return_value;
        }
    }
}
