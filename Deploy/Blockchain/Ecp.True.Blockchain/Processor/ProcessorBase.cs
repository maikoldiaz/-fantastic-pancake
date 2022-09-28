// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ProcessorBase.cs" company="Microsoft">
//   THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//   IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//   FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//   THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//   OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//   ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//   OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Blockchain.Processor
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using CommandLine;
    using Ecp.True.Blockchain.Entities;
    using Ecp.True.Blockchain.Helpers;
    using Ecp.True.Blockchain.SetUp;

    /// <summary>
    /// The ProcessorBase.
    /// </summary>
    public abstract class ProcessorBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ProcessorBase"/> class.
        /// </summary>
        protected ProcessorBase()
        {
        }

        /// <summary>
        /// Initializes the specified arguments.
        /// </summary>
        /// <param name="args">The arguments.</param>
        /// <param name="options">The options.</param>
        public void DoInitialize(string[] args, ConfigOptions options)
        {
            ArgumentValidators.ThrowIfNull(args, nameof(args));
            ArgumentValidators.ThrowIfNull(options, nameof(options));

            this.InitializeArgumentsVariables(args, options);
            this.ValidateArguments();
        }

        /// <summary>
        /// Validates the arguments.
        /// </summary>
        /// <param name="args">The arguments.</param>
        /// <exception cref="ArgumentException">Required Parameters are null.</exception>
        private void ValidateArguments()
        {
            if (Arguments.BlockchainAccount)
            {
                this.ValidateBlockchainAccountCreationArgument();
            }
            else
            {
                this.ValidateBlockchainSettingsArgument();
            }
        }

        /// <summary>
        /// Validates the blockchain account creation argument.
        /// </summary>
        /// <param name="args">The arguments.</param>
        /// <exception cref="ArgumentException">Required Parameters are null.</exception>
        private void ValidateBlockchainAccountCreationArgument()
        {
            Console.WriteLine($"Processing for Blockchain account creation...");
            Console.WriteLine($"Validating arguments...");

            if (string.IsNullOrEmpty(Arguments.AppId) ||
            string.IsNullOrEmpty(Arguments.AppSecret) ||
            string.IsNullOrEmpty(Arguments.KeyVaultUrl) ||
            string.IsNullOrEmpty(Arguments.EthereumAccountAddress) ||
            string.IsNullOrEmpty(Arguments.EthereumAccountSecret))
            {
                throw new ArgumentException("Required Parameters are null.");
            }
        }

        /// <summary>
        /// Validates the blockchain settings argument.
        /// </summary>
        /// <param name="args">The arguments.</param>
        /// <exception cref="ArgumentException">Required Parameters are null.</exception>
        private void ValidateBlockchainSettingsArgument()
        {
            Console.WriteLine($"Processing for Blockchain settings...");
            Console.WriteLine($"Validating arguments...");

            if (string.IsNullOrEmpty(Arguments.CompiledContractsLocation) ||
            string.IsNullOrEmpty(Arguments.EthereumAccountAddress) ||
            string.IsNullOrEmpty(Arguments.EthereumAccountSecret) ||
            string.IsNullOrEmpty(Arguments.RpcEndpoint) ||
            string.IsNullOrEmpty(Arguments.Localpath) ||
            string.IsNullOrEmpty(Arguments.StorageConnectionString) ||
            string.IsNullOrEmpty(Arguments.AppId) ||
            string.IsNullOrEmpty(Arguments.AppSecret) ||
            string.IsNullOrEmpty(Arguments.KeyVaultUrl))
            {
                throw new ArgumentException("Required Parameters are null.");
            }
        }

        private void InitializeArgumentsVariables(string[] args, ConfigOptions options)
        {
            if (Debugger.IsAttached)
            {
                Arguments.BlockchainAccount = options.BlockchainAccount;
                if (options.BlockchainAccount)
                {
                    Arguments.AppId = options.AppId;
                    Arguments.AppSecret = options.AppSecret;
                    Arguments.KeyVaultUrl = options.KeyVaultUrl;
                    Arguments.EthereumAccountAddress = options.EthereumAccountAddress;
                    Arguments.EthereumAccountSecret = options.EthereumAccountSecret;
                }
                else
                {
                    Arguments.CompiledContractsLocation = options.CompiledContractsLocation;
                    Arguments.EthereumAccountAddress = options.EthereumAccountAddress;
                    Arguments.EthereumAccountSecret = options.EthereumAccountSecret;
                    Arguments.RpcEndpoint = options.RpcEndpoint;
                    Arguments.Localpath = options.Localpath;
                    Arguments.StorageConnectionString = options.StorageConnectionString;
                    Arguments.AppId = options.AppId;
                    Arguments.AppSecret = options.AppSecret;
                    Arguments.KeyVaultUrl = options.KeyVaultUrl;
                    Arguments.TenantId = options.TenantId;
                    Arguments.ResourceId = options.ResourceId;
                }

                return;
            }

            Parser.Default.ParseArguments<ConfigOptions>(args)
                   .WithParsed(o =>
                   {
                       Arguments.BlockchainAccount = o.BlockchainAccount;
                       if (o.BlockchainAccount)
                       {
                           Arguments.AppId = o.AppId;
                           Arguments.AppSecret = o.AppSecret;
                           Arguments.KeyVaultUrl = o.KeyVaultUrl;
                           Arguments.EthereumAccountAddress = o.EthereumAccountAddress;
                           Arguments.EthereumAccountSecret = o.EthereumAccountSecret;
                       }
                       else
                       {
                           Arguments.CompiledContractsLocation = o.CompiledContractsLocation;
                           Arguments.EthereumAccountAddress = o.EthereumAccountAddress;
                           Arguments.EthereumAccountSecret = o.EthereumAccountSecret;
                           Arguments.RpcEndpoint = o.RpcEndpoint;
                           Arguments.Localpath = o.Localpath;
                           Arguments.StorageConnectionString = o.StorageConnectionString;
                           Arguments.AppId = o.AppId;
                           Arguments.AppSecret = o.AppSecret;
                           Arguments.KeyVaultUrl = o.KeyVaultUrl;
                           Arguments.TenantId = o.TenantId;
                           Arguments.ResourceId = o.ResourceId;
                       }
                   }).WithNotParsed(erros => this.HandleErrors(erros));
        }

        /// <summary>
        /// Handles the errors.
        /// </summary>
        /// <param name="errors">The errors.</param>
        private void HandleErrors(IEnumerable<Error> errors)
        {
            errors.ForEach(e =>
            {
                Console.WriteLine(e.ToString());
            });

            throw new ArgumentException("Required Parameters are null.");
        }
    }
}