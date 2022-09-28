// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ActionMenu.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Host.DataGenerator.Console
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    /// <summary>
    /// The ActionMenu.
    /// </summary>
    public class ActionMenu
    {
        /// <summary>
        /// The options.
        /// </summary>
        private readonly IList<Option> options;

        /// <summary>
        /// Initializes a new instance of the <see cref="ActionMenu" /> class.
        /// </summary>
        public ActionMenu()
        {
            this.options = new List<Option>();
        }

        /// <summary>
        /// Adds the specified option.
        /// </summary>
        /// <param name="option">The option.</param>
        /// <param name="callback">The callback.</param>
        /// <returns>Adds the call back action.</returns>
        public ActionMenu Add(string option, Func<Task> callback)
        {
            return this.Add(new Option(option, callback));
        }

        /// <summary>
        /// Adds the specified option.
        /// </summary>
        /// <param name="option">The option.</param>
        /// <returns>Adds the option.</returns>
        public ActionMenu Add(Option option)
        {
            this.options.Add(option);
            return this;
        }

        /// <summary>
        /// Displays the asynchronous.
        /// </summary>
        /// <param name="shouldReDisplayMenu">if set to <c>true</c> [should re display menu].</param>
        /// <returns>The task.</returns>
        public async Task DisplayAsync(bool shouldReDisplayMenu = false)
        {
            if (!shouldReDisplayMenu)
            {
                this.DisplayMenu();
            }

            int choice;
            do
            {
                if (shouldReDisplayMenu)
                {
                    Console.WriteLine($"{Environment.NewLine}Data Generation Application");
                    this.DisplayMenu();
                }

                choice = ReadInput($"Choose an option:", min: 1, max: this.options.Count + 1);
                if (choice != this.options.Count + 1)
                {
                    Console.WriteLine($"{Environment.NewLine}");
                    await this.options[choice - 1].ProcessCallbackAsync().ConfigureAwait(false);
                }
            }
            while (choice != this.options.Count + 1);
        }

        /// <summary>
        /// Reads the input.
        /// </summary>
        /// <param name="prompt">The prompt.</param>
        /// <param name="min">The minimum.</param>
        /// <param name="max">The maximum.</param>
        /// <returns>The integer.</returns>
        private static int ReadInput(string prompt, int min, int max)
        {
            DisplayPrompt(prompt);
            return ReadInput(min, max);
        }

        /// <summary>
        /// Reads the input.
        /// </summary>
        /// <param name="min">The minimum.</param>
        /// <param name="max">The maximum.</param>
        /// <returns>The integer.</returns>
        private static int ReadInput(int min, int max)
        {
            int value = ReadInput();

            while (value < min || value > max)
            {
                DisplayPrompt("Please enter an valid option between {0} and {1} (inclusive): ", min, max);
                value = ReadInput();
            }

            return value;
        }

        /// <summary>
        /// Reads the input.
        /// </summary>
        /// <returns>The integer.</returns>
        private static int ReadInput()
        {
            string input = Console.ReadLine();
            int value;

            while (!int.TryParse(input, out value))
            {
                DisplayPrompt("Please enter an valid input: ");
                input = Console.ReadLine();
            }

            return value;
        }

        /// <summary>
        /// Displays the prompt.
        /// </summary>
        /// <param name="format">The format.</param>
        /// <param name="args">The arguments.</param>
        private static void DisplayPrompt(string format, params object[] args)
        {
            format = format.Trim() + " ";
            Console.Write(format, args);
        }

        private void DisplayMenu()
        {
            var index = 1;
            foreach (var option in this.options)
            {
                Console.WriteLine($"{index++}. {option.Name}");
            }

            Console.WriteLine($"{this.options.Count + 1}. Exit{Environment.NewLine}");
        }
    }
}
