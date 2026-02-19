using System;
using System.Collections.Generic;
using System.Text;

namespace StoQ.App.Models;

public class NodeModel
{
    public string Id { get; set; }
    public string ParentId { get; set; }
    public string Type { get; set; } // HOUSE, STORAGE, ITEM
    public string Name { get; set; }
    public string QrCode { get; set; }
    public Dictionary<string, object> Properties { get; set; }

    // UI 표시용 읽기 전용 속성
    public string TypeImage => Type switch
    {
        "HOUSE" => "house_icon.png",
        "STORAGE" => "box_icon.png",
        _ => "item_icon.png"
    };
}
