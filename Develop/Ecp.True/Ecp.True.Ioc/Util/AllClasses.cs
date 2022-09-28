// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AllClasses.cs" company="Microsoft">
//   THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//   IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//   FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//   THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//   OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//   ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//   OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Ioc
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Security;

    /// <summary>
    /// The utility class to fetch all valid components in assemblies.
    /// </summary>
    public static class AllClasses
    {
        /// <summary>
        /// The net framework product name.
        /// </summary>
        private static readonly string NetFrameworkProductName = GetNetFrameworkProductName();

        /// <summary>
        /// Returns all visible, non-abstract classes from <paramref name="assemblies" />.
        /// </summary>
        /// <param name="assemblies">The assemblies.</param>
        /// <returns>
        /// All visible, non-abstract classes.
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="assemblies" /> is <see langword="null" />.</exception>
        /// <exception cref="ArgumentException"><paramref name="assemblies" /> contains <see langword="null" /> elements.</exception>
        /// <remarks>
        /// If <paramref name="skipOnError" /> is <see langword="true" />, all exceptions thrown while getting types from the assemblies are ignored, and the types
        /// that can be retrieved are returned; otherwise, the original exception is thrown.
        /// </remarks>
        public static IEnumerable<Type> FromAssemblies(IEnumerable<Assembly> assemblies)
        {
            return FromAssemblies(assemblies, true);
        }

        /// <summary>
        /// Returns all visible, non-abstract classes from <paramref name="assemblies" />.
        /// </summary>
        /// <param name="assemblies">The assemblies.</param>
        /// <param name="skipOnError"><see langword="true" /> to skip errors; otherwise, <see langword="true" />.</param>
        /// <returns>
        /// All visible, non-abstract classes.
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="assemblies" /> is <see langword="null" />.</exception>
        /// <exception cref="ArgumentException"><paramref name="assemblies" /> contains <see langword="null" /> elements.</exception>
        /// <remarks>
        /// If <paramref name="skipOnError" /> is <see langword="true" />, all exceptions thrown while getting types from the assemblies are ignored, and the types
        /// that can be retrieved are returned; otherwise, the original exception is thrown.
        /// </remarks>
        public static IEnumerable<Type> FromAssemblies(IEnumerable<Assembly> assemblies, bool skipOnError)
        {
            return FromCheckedAssemblies(CheckAssemblies(assemblies ?? throw new ArgumentNullException(nameof(assemblies))), skipOnError);
        }

        /// <summary>
        /// Returns all visible, non-abstract classes from all assemblies that are located in the base folder of the current application domain.
        /// </summary>
        /// <returns>
        /// All visible, non-abstract classes.
        /// </returns>
        /// <remarks>
        /// If <paramref name="skipOnError" /> is <see langword="true" />, all exceptions thrown while loading assemblies or getting types from the assemblies are ignored, and the types
        /// that can be retrieved are returned; otherwise, the original exception is thrown.
        /// </remarks>
        public static IEnumerable<Type> FromAssembliesInBasePath()
        {
            return FromAssembliesInBasePath(false, true);
        }

        /// <summary>
        /// Returns all visible, non-abstract classes from all assemblies that are located in the base folder of the current application domain.
        /// </summary>
        /// <param name="includeSystemAssemblies"><see langword="false" /> to include system assemblies; otherwise, <see langword="false" />. Defaults to <see langword="false" />.</param>
        /// <param name="skipOnError"><see langword="true" /> to skip errors; otherwise, <see langword="true" />.</param>
        /// <returns>
        /// All visible, non-abstract classes.
        /// </returns>
        /// <remarks>
        /// If <paramref name="skipOnError" /> is <see langword="true" />, all exceptions thrown while loading assemblies or getting types from the assemblies are ignored, and the types
        /// that can be retrieved are returned; otherwise, the original exception is thrown.
        /// </remarks>
        public static IEnumerable<Type> FromAssembliesInBasePath(bool includeSystemAssemblies, bool skipOnError)
        {
            return FromCheckedAssemblies(GetAssembliesInBasePath(includeSystemAssemblies, skipOnError), skipOnError);
        }

        /// <summary>
        /// Checks the assemblies.
        /// </summary>
        /// <param name="assemblies">The assemblies to scan.</param>
        /// <returns>The assemblies list.</returns>
        /// <exception cref="ArgumentException">Assembly cannot be null - assemblies.</exception>
        private static IEnumerable<Assembly> CheckAssemblies(IEnumerable<Assembly> assemblies)
        {
            var checkAssemblies = assemblies as Assembly[] ?? assemblies.ToArray();
            if (checkAssemblies.Any(assembly => assembly == null))
            {
                throw new ArgumentException("Assembly cannot be null", nameof(assemblies));
            }

            return checkAssemblies;
        }

        /// <summary>
        /// From the checked assemblies.
        /// </summary>
        /// <param name="assemblies">The assemblies.</param>
        /// <param name="skipOnError">if set to <c>true</c> [skip on error].</param>
        /// <returns>The list of types.</returns>
        private static IEnumerable<Type> FromCheckedAssemblies(IEnumerable<Assembly> assemblies, bool skipOnError)
        {
            return assemblies
                    .SelectMany(a =>
                    {
                        IEnumerable<TypeInfo> types;

                        try
                        {
                            types = a.DefinedTypes;
                        }
                        catch (ReflectionTypeLoadException e)
                        {
                            if (!skipOnError)
                            {
                                throw;
                            }

                            types = e.Types.TakeWhile(t => t != null).Select(t => t.GetTypeInfo());
                        }

                        return types.Where(ti => ti.IsClass && !ti.IsAbstract && !ti.IsValueType && ti.IsVisible).Select(ti => ti.AsType());
                    });
        }

        /// <summary>
        /// Gets the assemblies in base path.
        /// </summary>
        /// <param name="includeSystemAssemblies">if set to <c>true</c> [include system assemblies].</param>
        /// <param name="skipOnError">if set to <c>true</c> [skip on error].</param>
        /// <returns>The assemblies.</returns>
        private static IEnumerable<Assembly> GetAssembliesInBasePath(bool includeSystemAssemblies, bool skipOnError)
        {
            string basePath;

            try
            {
                basePath = AppDomain.CurrentDomain.BaseDirectory;
            }
            catch (SecurityException)
            {
                if (!skipOnError)
                {
                    throw;
                }

                return Array.Empty<Assembly>();
            }

            return GetAssemblyNames(basePath, skipOnError)
                    .Select(an => LoadAssembly(Path.GetFileNameWithoutExtension(an), skipOnError))
                    .Where(a => a != null && (includeSystemAssemblies || !IsSystemAssembly(a)));
        }

        /// <summary>
        /// Gets the assembly names.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <param name="skipOnError">if set to <c>true</c> [skip on error].</param>
        /// <returns>The list.</returns>
        private static IEnumerable<string> GetAssemblyNames(string path, bool skipOnError)
        {
            try
            {
                return Directory.EnumerateFiles(path, "*.dll").Concat(Directory.EnumerateFiles(path, "*.exe"));
            }
            catch (Exception e)
            {
                var hasException = e is DirectoryNotFoundException || e is IOException || e is SecurityException || e is UnauthorizedAccessException;
                if (!(skipOnError && hasException))
                {
                    throw;
                }

                return Array.Empty<string>();
            }
        }

        /// <summary>
        /// Gets the name of the net framework product.
        /// </summary>
        /// <returns>The name.</returns>
        private static string GetNetFrameworkProductName()
        {
            var productAttribute = typeof(object).GetTypeInfo().Assembly.GetCustomAttribute<AssemblyProductAttribute>();
            return productAttribute?.Product;
        }

        /// <summary>
        /// Determines whether [is system assembly] [the specified a].
        /// </summary>
        /// <param name="a">The assembly.</param>
        /// <returns>
        /// <c>true</c> if [is system assembly] [the specified a]; otherwise, <c>false</c>.
        /// </returns>
        private static bool IsSystemAssembly(Assembly a)
        {
            if (NetFrameworkProductName == null)
            {
                return false;
            }

            var productAttribute = a.GetCustomAttribute<AssemblyProductAttribute>();
            return productAttribute != null && string.Compare(NetFrameworkProductName, productAttribute.Product, StringComparison.Ordinal) == 0;
        }

        /// <summary>
        /// Loads the assembly.
        /// </summary>
        /// <param name="assemblyName">Name of the assembly.</param>
        /// <param name="skipOnError">if set to <c>true</c> [skip on error].</param>
        /// <returns>The assembly.</returns>
        private static Assembly LoadAssembly(string assemblyName, bool skipOnError)
        {
            try
            {
                return Assembly.Load(assemblyName);
            }
            catch (Exception e)
            {
                if (!(skipOnError && (e is FileNotFoundException || e is FileLoadException || e is BadImageFormatException)))
                {
                    throw;
                }

                return null;
            }
        }
    }
}