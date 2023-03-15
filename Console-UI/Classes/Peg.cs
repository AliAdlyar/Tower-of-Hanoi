using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Console_UI.Classes
{
	internal class Peg
	{
		Stack<int> disks;

		public Peg(Stack<int> disks)
		{
			this.disks = disks;
		}
	}
}
