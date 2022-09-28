// <copyright file="Keys.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>

namespace Ecp.True.Bdd.Tests.Entities
{
    public static class Keys
    {
        public static readonly string EntityType = nameof(EntityType);
        public static readonly string Route = nameof(Route);
        public static readonly string Result = nameof(Result);
        public static readonly string Results = nameof(Results);
        public static readonly string Error = nameof(Error);
        public static readonly string ErrorStatus = nameof(ErrorStatus);
        public static readonly string Status = nameof(Status);
        public static readonly string InitialRowCount = nameof(InitialRowCount);
        public static readonly string RandomFieldValue = nameof(RandomFieldValue);
        public static readonly string Field = nameof(Field);
        public static readonly string HomologationGroupId = nameof(HomologationGroupId);
        public static readonly string HomologationId = nameof(HomologationId);
        public static readonly string SelectedValue = nameof(SelectedValue);
        public static readonly string Category = nameof(Category);
        public static readonly string Title = nameof(Title);

        public static string Enter { get; internal set; }
    }
}