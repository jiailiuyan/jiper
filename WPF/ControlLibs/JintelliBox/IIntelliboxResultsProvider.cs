using System.Collections.Generic;
using System.Collections;

namespace JintelliBox
{
    /// <summary>
    ///  Defines the interface contract between an <see cref="Intellibox" /> and its <see cref="Intellibox.DataProvider" />.
    /// </summary>
    public interface IIntelliboxResultsProvider
    {
        /// <summary>
        ///  Tell the <see cref="IIntelliboxResultsProvider" /> to search for the <paramref name="searchTerm" />.
        /// </summary>
        /// <param name="searchTerm"> The text in the search box at the time the search was requested. </param>
        /// <param name="maxResults">
        ///  The maximum number of search results the <see cref="Intellibox" /> wants returned.
        /// </param>
        /// <param name="extraInfo"> 
        ///  This is the value of the Tag property of the <see cref="Intellibox" /> control at the
        ///  time the search was started. Use the Tag property to pass any custom data to your <see cref="IIntelliboxResultsProvider" />.
        /// </param>
        IEnumerable DoSearch(string searchTerm, int maxResults, object extraInfo);

        IEnumerable Datas { get; }
    }
}