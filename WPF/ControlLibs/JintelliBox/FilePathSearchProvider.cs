//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.IO;

//namespace JintelliBox
//{
//    /// <summary>
//    ///  A default data provider for the <see cref="Intellibox" />. This data provider searches the
//    ///  local filesystem for Files or Directories or both whose fully-qualified path matches the
//    ///  partial path entered by the user.
//    /// </summary>
//    public class FilePathSearchProvider : IIntelliboxResultsProvider
//    {
//        /// <summary> Enumeration of the search options for a <see cref="FilePathSearchProvider" /> </summary>
//        public enum PathSearchIncludes
//        {
//            /// <summary> Search for matching Files and for matching Directories. </summary>
//            FilesAndDirectories,

//            /// <summary> Search for matching Files, but not for matching Directories. </summary>
//            FilesOnly,

//            /// <summary> Search for matching Directories, but not for matching Files. </summary>
//            DirectoriesOnly
//        }

//        /// <summary>
//        ///  Whether this <see cref="FilePathSearchProvider" /> should search for Directories, Files,
//        ///  or both.
//        /// </summary>
//        public PathSearchIncludes IncludeInSearch { get; set; }

//        /// <summary> Creates a new <see cref="FilePathSearchProvider" /> </summary>
//        /// <param name="searchIncludes">
//        ///  Whether this provider should search for Directories, Files, or both.
//        /// </param>
//        public FilePathSearchProvider(PathSearchIncludes searchIncludes = PathSearchIncludes.FilesAndDirectories)
//        {
//            IncludeInSearch = searchIncludes;
//        }

//        /// <summary>
//        ///  Searches for Files and/or Directories on the local filesystem whose fully-qualified path
//        ///  matched the <paramref name="searchTerm" />. Directories are always returned before files.
//        /// </summary>
//        /// <param name="searchTerm">
//        ///  The search path. The function will return all the files and/or directories under this path.
//        /// </param>
//        /// <param name="maxResults">
//        ///  The maximum number of results to return. The function will return all results when this
//        ///  is 0 (zero).
//        /// </param>
//        /// <param name="extraInfo">  Not used. </param>
//        /// <returns>
//        ///  A list of Files or Directories whose fully-qualified path starts with the <paramref name="searchTerm" />.
//        /// </returns>
//        public System.Collections.IEnumerable DoSearch(string searchTerm, int maxResults, object extraInfo)
//        {
//            if (string.IsNullOrEmpty(searchTerm))
//                return new string[0];

//            bool includeDirs = IncludeInSearch == PathSearchIncludes.DirectoriesOnly || IncludeInSearch == PathSearchIncludes.FilesAndDirectories;
//            bool includeFiles = IncludeInSearch == PathSearchIncludes.FilesOnly || IncludeInSearch == PathSearchIncludes.FilesAndDirectories;

//            var basePath = Path.GetDirectoryName(searchTerm) ?? searchTerm;
//            var fileName = Path.GetFileName(searchTerm) + "*";

//            if (!Directory.Exists(basePath))
//            {
//                return new string[0];
//            }

//            var searchResults = new List<string>();
//            if (includeDirs)
//            {
//                searchResults.AddRange(Directory.GetDirectories(basePath, fileName));
//            }

//            if (includeFiles)
//            {
//                searchResults.AddRange(Directory.GetFiles(basePath, fileName));
//            }

//            if (maxResults > 0)
//            {
//                return searchResults.Take(maxResults);
//            }

//            return searchResults;
//        }
//    }
//}