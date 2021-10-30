using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SOLID
{
    class GuessTheNumberGame : IGame
    {
		private INumberGenerator	_numberGenerator;
		private IInterface			_interface;
		private int					_attempts;		
		private bool				_play = true;
		private int					_number;

		public GuessTheNumberGame(INumberGenerator numberGenerator,	IInterface curInterface)
		{
			_numberGenerator	= numberGenerator;			
			_interface			= curInterface;
		}

		private void Init()
        {
			_interface.Write($"Enter a number between {AppSettings.GetMin()} and {AppSettings.GetMax()}. You have {_attempts} to guess the number.");

			//example of using NumberGeneratorToInterface instead of NumberGenerator - commented code can be added or not
			//var n = (NumberGeneratorToInterface)_numberGenerator;
			//n.setInterface(_interface);

			_number = _numberGenerator.Generate();
			_attempts = AppSettings.GetAttempts();
		}
		public void Play()
        {
			while (_play)
			{
				Init();

				while (_attempts > 0)
				{
					Try(_interface.Read());
				}
			}
        }

        private void Try(int userNumber)
        {
			_attempts--;

			if (userNumber == _number)
			{
				ContinueDialog(true);
			}
			else
			{
				if (_attempts > 0)
				{
					var hint = userNumber > _number ? "less" : "greater";
					_interface.Write($"The number is {hint} than {userNumber}. {_attempts} attempts left.");
				}
				else
                {
					ContinueDialog(false);
				}
			}
		}

		private void ContinueDialog(bool win)
        {
			_attempts = 0;
			_interface.Write(win ? "You win!" : "Game over.");
			_interface.Write("Please press '0' to play again or '1'-'9' to exit.");

			if (_interface.Read() != 0)
            {				
				_play = false;
            }
		}
    }
}
