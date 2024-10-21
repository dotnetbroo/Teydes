using Teydes.Domain.Commons;
using Teydes.Domain.Entities.Quizes;

namespace Teydes.Domain.Entities.Assets;

public class Asset : Auditable
{
    public string Path { get; set; }

}
