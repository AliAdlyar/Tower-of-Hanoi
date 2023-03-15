using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace Console_UI
{
	public partial class UI : Window
	{
		#region properties
		private int disks = 0;
		private string str = "";
		private char[] stringArray;
		private List<string> stepsList = new List<string>();
		private Stack<Rectangle> startDiskStack = new Stack<Rectangle>();
		private Stack<Rectangle> tempDiskStack = new Stack<Rectangle>();
		private Stack<Rectangle> endDiskStack = new Stack<Rectangle>();
		private Rectangle currDisk = new Rectangle();
		private int? state = null;
		private bool isActive = false;
		private Thickness margin, m1, m2, m3;
		private List<ThicknessAnimation> marginAnimationList = new List<ThicknessAnimation>();
		private List<DoubleAnimation> fadeAnimationList = new List<DoubleAnimation>();
		private List<Rectangle> fadeDiskList = new List<Rectangle>();
		private ThicknessAnimation upAnimation = new ThicknessAnimation();
		private ThicknessAnimation navAnimation = new ThicknessAnimation();
		private ThicknessAnimation downAnimation = new ThicknessAnimation();
		#endregion

		public UI()
		{
			InitializeComponent();
			Activate();
		}

		private void SolveTowers(int n, char startPeg, char tempPeg, char endPeg)
		{
			if (n < 1)
				return;
			SolveTowers(n - 1, startPeg, endPeg, tempPeg);

			str = $"{startPeg}{endPeg}";
			stepsList.Add(str);

			SolveTowers(n - 1, tempPeg, startPeg, endPeg);
		}

		private void PerformAnimations()
		{
			marginAnimationList[0].Completed += delegate
			{
				// Start the other animation after the end of the previous animation.
				if (marginAnimationList.Count == 0)
				{
					currDisk.BeginAnimation(MarginProperty, null);
					currDisk.Margin = margin;
					return;
				}
				PerformAnimations();
			};
			currDisk.BeginAnimation(MarginProperty, marginAnimationList[0]);
			marginAnimationList.Remove(marginAnimationList[0]);
		}

		private void PerformAnimations(List<Rectangle> rec, DependencyProperty property, List<DoubleAnimation> list)
		{
			if (list.Count == 0)
				return;
			list[0].Completed += delegate
			{
				// Start the other animation after the end of the previous animation.
				PerformAnimations(rec, property, list);
			};
			rec[0].BeginAnimation(property, list[0]);
			list.Remove(list[0]);
			rec.Remove(rec[0]);
		}

		private void FadeDisks()
		{
			Stack<Rectangle> stack = new Stack<Rectangle>();
			for (int i = 0; i < 9; i++)
				stack.Push(startDiskStack.Pop());

			fadeAnimationList.Clear();
			fadeDiskList.Clear();
			for (int i = 0; i < 9; i++)
			{
				DoubleAnimation dAn = new DoubleAnimation()
				{
					From = 0,
					To = 1,
					Duration = new Duration(TimeSpan.FromSeconds(0.2))
				};
				fadeAnimationList.Add(dAn);
				fadeDiskList.Add(stack.Pop());
			}
			PerformAnimations(fadeDiskList, OpacityProperty, fadeAnimationList);
		}

		private void Resort()
		{
			startDiskStack.Clear();
			tempDiskStack.Clear();
			endDiskStack.Clear();

			startDiskStack.Push(Disk1);
			Disk1.BeginAnimation(OpacityProperty, null);
			Disk1.Opacity = 1;
			Disk1.Margin = new Thickness(-300, -100, 0, 0);
			startDiskStack.Push(Disk2);
			Disk2.BeginAnimation(OpacityProperty, null);
			Disk2.Opacity = 1;
			Disk2.Margin = new Thickness(-300, -100, 0, 35);
			startDiskStack.Push(Disk3);
			Disk3.BeginAnimation(OpacityProperty, null);
			Disk3.Opacity = 1;
			Disk3.Margin = new Thickness(-300, -100, 0, 70);
			startDiskStack.Push(Disk4);
			Disk4.BeginAnimation(OpacityProperty, null);
			Disk4.Opacity = 1;
			Disk4.Margin = new Thickness(-300, -100, 0, 105);
			startDiskStack.Push(Disk5);
			Disk5.BeginAnimation(OpacityProperty, null);
			Disk5.Opacity = 1;
			Disk5.Margin = new Thickness(-300, -100, 0, 140);
			startDiskStack.Push(Disk6);
			Disk6.BeginAnimation(OpacityProperty, null);
			Disk6.Opacity = 1;
			Disk6.Margin = new Thickness(-300, -100, 0, 175);
			startDiskStack.Push(Disk7);
			Disk7.BeginAnimation(OpacityProperty, null);
			Disk7.Opacity = 1;
			Disk7.Margin = new Thickness(-300, -100, 0, 210);
			startDiskStack.Push(Disk8);
			Disk8.BeginAnimation(OpacityProperty, null);
			Disk8.Opacity = 1;
			Disk8.Margin = new Thickness(-300, -100, 0, 245);
			startDiskStack.Push(Disk9);
			Disk9.BeginAnimation(OpacityProperty, null);
			Disk9.Opacity = 1;
			Disk9.Margin = new Thickness(-300, -100, 0, 280);

			LeftBtn.Visibility = Visibility.Hidden;
			RightBtn.Visibility = Visibility.Hidden;
			CurrentStepTB.Text = "";
			stepsList.Clear();
		}

		private bool ChangeState(string step)
		{
			if (isActive)
			{
				currDisk.BeginAnimation(MarginProperty, null);
				currDisk.Margin = margin;
				marginAnimationList.Clear();
				isActive = false;
				return false;
			}
			switch (step[0])
			{
				case 'A':
					{
						currDisk = startDiskStack.Pop();
						margin = currDisk.Margin;
						switch (step[1])
						{
							case 'B':
								{
									tempDiskStack.Push(currDisk);
									m1 = margin;
									m1.Bottom += (8 - startDiskStack.Count) * 35 + 120;
									upAnimation = new ThicknessAnimation()
									{
										From = margin,
										To = m1,
										Duration = new Duration(TimeSpan.FromSeconds(0.4)),
										DecelerationRatio = 0.6
									};
									marginAnimationList.Add(upAnimation);

									m2 = m1;
									m2.Right += -600;
									navAnimation = new ThicknessAnimation()
									{
										From = m1,
										To = m2,
										Duration = new Duration(TimeSpan.FromSeconds(0.5)),
										DecelerationRatio = 0.6
									};
									marginAnimationList.Add(navAnimation);

									m3 = m2;
									m3.Bottom -= (9 - tempDiskStack.Count) * 35 + 120;
									downAnimation = new ThicknessAnimation()
									{
										From = m2,
										To = m3,
										Duration = new Duration(TimeSpan.FromSeconds(0.4)),
										DecelerationRatio = 0.6
									};
									marginAnimationList.Add(downAnimation);

									downAnimation.Completed += delegate { isActive = false; };
									margin = (Thickness)marginAnimationList[marginAnimationList.Count - 1].To;
									PerformAnimations();
									break;
								}

							case 'C':
								{
									endDiskStack.Push(currDisk);
									m1 = margin;
									m1.Bottom += (8 - startDiskStack.Count) * 35 + 120;
									upAnimation = new ThicknessAnimation()
									{
										From = margin,
										To = m1,
										Duration = new Duration(TimeSpan.FromSeconds(0.4)),
										DecelerationRatio = 0.6
									};
									marginAnimationList.Add(upAnimation);

									m2 = m1;
									m2.Right += -1200;
									navAnimation = new ThicknessAnimation()
									{
										From = m1,
										To = m2,
										Duration = new Duration(TimeSpan.FromSeconds(0.5)),
										DecelerationRatio = 0.6
									};
									marginAnimationList.Add(navAnimation);

									m3 = m2;
									m3.Bottom -= (9 - endDiskStack.Count) * 35 + 120;
									downAnimation = new ThicknessAnimation()
									{
										From = m2,
										To = m3,
										Duration = new Duration(TimeSpan.FromSeconds(0.4)),
										DecelerationRatio = 0.6
									};
									marginAnimationList.Add(downAnimation);

									downAnimation.Completed += delegate { isActive = false; };
									margin = (Thickness)marginAnimationList[marginAnimationList.Count - 1].To;
									PerformAnimations();
									break;
								}
						}
						break;
					}
				case 'B':
					{
						currDisk = tempDiskStack.Pop();
						margin = currDisk.Margin;
						switch (step[1])
						{
							case 'A':
								{
									startDiskStack.Push(currDisk);
									m1 = margin;
									m1.Bottom += (8 - tempDiskStack.Count) * 35 + 120;
									upAnimation = new ThicknessAnimation()
									{
										From = margin,
										To = m1,
										Duration = new Duration(TimeSpan.FromSeconds(0.4)),
										DecelerationRatio = 0.6
									};
									marginAnimationList.Add(upAnimation);

									m2 = m1;
									m2.Right += 600;
									navAnimation = new ThicknessAnimation()
									{
										From = m1,
										To = m2,
										Duration = new Duration(TimeSpan.FromSeconds(0.5)),
										DecelerationRatio = 0.6
									};
									marginAnimationList.Add(navAnimation);

									m3 = m2;
									m3.Bottom -= (9 - startDiskStack.Count) * 35 + 120;
									downAnimation = new ThicknessAnimation()
									{
										From = m2,
										To = m3,
										Duration = new Duration(TimeSpan.FromSeconds(0.4)),
										DecelerationRatio = 0.6
									};
									marginAnimationList.Add(downAnimation);

									downAnimation.Completed += delegate { isActive = false; };
									margin = (Thickness)marginAnimationList[marginAnimationList.Count - 1].To;
									PerformAnimations();
									break;
								}

							case 'C':
								{
									endDiskStack.Push(currDisk);
									m1 = margin;
									m1.Bottom += (8 - tempDiskStack.Count) * 35 + 120;
									upAnimation = new ThicknessAnimation()
									{
										From = margin,
										To = m1,
										Duration = new Duration(TimeSpan.FromSeconds(0.4)),
										DecelerationRatio = 0.6
									};
									marginAnimationList.Add(upAnimation);

									m2 = m1;
									m2.Right += -600;
									navAnimation = new ThicknessAnimation()
									{
										From = m1,
										To = m2,
										Duration = new Duration(TimeSpan.FromSeconds(0.5)),
										DecelerationRatio = 0.6
									};
									marginAnimationList.Add(navAnimation);

									m3 = m2;
									m3.Bottom -= (9 - endDiskStack.Count) * 35 + 120;
									downAnimation = new ThicknessAnimation()
									{
										From = m2,
										To = m3,
										Duration = new Duration(TimeSpan.FromSeconds(0.4)),
										DecelerationRatio = 0.6
									};
									marginAnimationList.Add(downAnimation);

									downAnimation.Completed += delegate { isActive = false; };
									margin = (Thickness)marginAnimationList[marginAnimationList.Count - 1].To;
									PerformAnimations();
									break;
								}
						}
						break;
					}
				case 'C':
					{
						currDisk = endDiskStack.Pop();
						margin = currDisk.Margin;
						switch (step[1])
						{
							case 'A':
								{
									startDiskStack.Push(currDisk);
									m1 = margin;
									m1.Bottom += (8 - endDiskStack.Count) * 35 + 120;
									upAnimation = new ThicknessAnimation()
									{
										From = margin,
										To = m1,
										Duration = new Duration(TimeSpan.FromSeconds(0.4)),
										DecelerationRatio = 0.6
									};
									marginAnimationList.Add(upAnimation);

									m2 = m1;
									m2.Right += 1200;
									navAnimation = new ThicknessAnimation()
									{
										From = m1,
										To = m2,
										Duration = new Duration(TimeSpan.FromSeconds(0.5)),
										DecelerationRatio = 0.6
									};
									marginAnimationList.Add(navAnimation);

									m3 = m2;
									m3.Bottom -= (9 - startDiskStack.Count) * 35 + 120;
									downAnimation = new ThicknessAnimation()
									{
										From = m2,
										To = m3,
										Duration = new Duration(TimeSpan.FromSeconds(0.4)),
										DecelerationRatio = 0.6
									};
									marginAnimationList.Add(downAnimation);

									downAnimation.Completed += delegate { isActive = false; };
									margin = (Thickness)marginAnimationList[marginAnimationList.Count - 1].To;
									PerformAnimations();
									break;
								}

							case 'B':
								{
									tempDiskStack.Push(currDisk);
									m1 = margin;
									m1.Bottom += (8 - endDiskStack.Count) * 35 + 120;
									upAnimation = new ThicknessAnimation()
									{
										From = margin,
										To = m1,
										Duration = new Duration(TimeSpan.FromSeconds(0.4)),
										DecelerationRatio = 0.6
									};
									marginAnimationList.Add(upAnimation);

									m2 = m1;
									m2.Right += 600;
									navAnimation = new ThicknessAnimation()
									{
										From = m1,
										To = m2,
										Duration = new Duration(TimeSpan.FromSeconds(0.5)),
										DecelerationRatio = 0.6
									};
									marginAnimationList.Add(navAnimation);

									m3 = m2;
									m3.Bottom -= (9 - tempDiskStack.Count) * 35 + 120;
									downAnimation = new ThicknessAnimation()
									{
										From = m2,
										To = m3,
										Duration = new Duration(TimeSpan.FromSeconds(0.4)),
										DecelerationRatio = 0.6
									};
									marginAnimationList.Add(downAnimation);

									downAnimation.Completed += delegate { isActive = false; };
									margin = (Thickness)marginAnimationList[marginAnimationList.Count - 1].To;
									PerformAnimations();
									break;
								}
						}
						break;
					}
			}
			isActive = true;
			return true;
		}

		private void ShowStepsWindow_Loaded(object sender, RoutedEventArgs e)
		{
			CurrentStepTB.Text = "";
			TotalstepsTB.Text = "";
			Resort();
			foreach (var x in startDiskStack)
				x.Opacity = 0;
			DisksText.Focus();

			margin = Base.Margin;
			m1 = margin;
			m1.Left -= 2200;
			navAnimation = new ThicknessAnimation()
			{
				From = m1,
				To = margin,
				Duration = new Duration(TimeSpan.FromSeconds(2)),
				DecelerationRatio = 1
			};
			(Base).BeginAnimation(MarginProperty, navAnimation);

			margin = Peg1.Margin;
			m1 = margin;
			m1.Left -= 2200;
			navAnimation = new ThicknessAnimation()
			{
				From = m1,
				To = margin,
				Duration = new Duration(TimeSpan.FromSeconds(2)),
				DecelerationRatio = 1
			};
			(Peg1).BeginAnimation(MarginProperty, navAnimation);

			m2 = Peg2.Margin;
			m3 = margin;
			m3.Left -= 2200;
			navAnimation = new ThicknessAnimation()
			{
				From = m3,
				To = m2,
				Duration = new Duration(TimeSpan.FromSeconds(2)),
				DecelerationRatio = 1
			};
			(Peg2).BeginAnimation(MarginProperty, navAnimation);

			Thickness m4 = Peg3.Margin;
			Thickness m5 = m4;
			m5.Left -= 2200;
			navAnimation = new ThicknessAnimation()
			{
				From = m5,
				To = m4,
				Duration = new Duration(TimeSpan.FromSeconds(2)),
				DecelerationRatio = 1
			};
			navAnimation.Completed += delegate
			{
				FadeDisks();
			};
			(Peg3).BeginAnimation(MarginProperty, navAnimation);
		}

		private void OkBtn_Click(object sender, RoutedEventArgs e)
		{
			try
			{
				disks = Convert.ToInt32(DisksText.Text);
				if (disks > 9 || disks < 1)
				{
					MessageBox.Show("Choose a number between 1 to 9", "Invalid Number!");
					return;
				}
				DisksText.Text = "";
			}
			catch (Exception) { DisksText.Text = ""; return; }

			Resort();
			for (int i = 9; i > disks; i--)
			{
				DoubleAnimation dAn = new DoubleAnimation()
				{
					From = 1,
					To = 0,
					Duration = new Duration(TimeSpan.FromSeconds(0.6)),
					AccelerationRatio = 0.5,
					DecelerationRatio = 0.5
				};
				startDiskStack.Pop().BeginAnimation(OpacityProperty, dAn);
			}
			state = 0;
			SolveTowers(disks, 'A', 'B', 'C');
			TotalstepsTB.Text = $"total steps: {stepsList.Count}";

			RightBtn.Visibility = Visibility.Visible;
			OkBtn.Focus();
		}

		private void ShowStepsWindow_KeyDown(object sender, KeyEventArgs e)
		{
			if (state == null)
			{
				return;
			}
			if (e.Key == Key.Right)
			{
				e.Handled = true;
				if (RightBtn.Visibility == Visibility.Hidden)
					return;

				state++;
				if (ChangeState(stepsList[Convert.ToInt32(state - 1)]) == false)
				{
					state--;
					return;
				}
				CurrentStepTB.Text = $"Step: {state}";

				if (state == stepsList.Count)
					RightBtn.Visibility = Visibility.Hidden;
				LeftBtn.Visibility = Visibility.Visible;
			}
			if (e.Key == Key.Left)
			{
				e.Handled = true;
				if (LeftBtn.Visibility == Visibility.Hidden)
					return;

				stringArray = stepsList[Convert.ToInt32(state - 1)].ToCharArray();
				Array.Reverse(stringArray);
				str = stringArray[0].ToString() + stringArray[1].ToString();
				if (ChangeState(str) == false)
					return;
				state--;
				CurrentStepTB.Text = $"Step: {state}";

				if (state == 0)
				{
					LeftBtn.Visibility = Visibility.Hidden;
					CurrentStepTB.Text = "";
				}
				RightBtn.Visibility = Visibility.Visible;
			}
		}

		private void RightBtn_Click(object sender, RoutedEventArgs e)
		{
			InputManager.Current.ProcessInput(new KeyEventArgs(Keyboard.PrimaryDevice,
			Keyboard.PrimaryDevice.ActiveSource, 0, Key.Right)
			{
				RoutedEvent = Keyboard.KeyDownEvent
			});
		}

		private void LeftBtn_Click(object sender, RoutedEventArgs e)
		{
			InputManager.Current.ProcessInput(new KeyEventArgs(Keyboard.PrimaryDevice,
			Keyboard.PrimaryDevice.ActiveSource, 0, Key.Left)
			{
				RoutedEvent = Keyboard.KeyDownEvent
			});
		}
	}
}
