using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Managers
{
    public class PointsManager
    {
        public int points { get; private set; }

        public PointsManager()
        {
            points = 0;
        }
        public void HandleAnswer(bool isAnswerCorrect)
        {
            points += isAnswerCorrect ? 1 : 0;
        }
    }
}
