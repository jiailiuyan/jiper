using System.Collections.ObjectModel;

namespace JintelliBox
{
    /// <summary>
    ///  Represents an observable collection of <see cref="IntelliboxColumn" /> s. This class exists
    ///  becuase XAML pre-2009 spec doesn't support the instantiation of generic types.
    /// </summary>
    public class IntelliboxColumnCollection : ObservableCollection<IntelliboxColumn>
    {
    }
}