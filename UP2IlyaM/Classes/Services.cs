using System;
using System.Drawing;
using Org.BouncyCastle.Asn1.X509;
using Bitmap = Avalonia.Media.Imaging.Bitmap;
using Time = Org.BouncyCastle.Asn1.Cms.Time;

namespace UP2IlyaM.Classes;

public class Services
{
    public int ID { get; set; }
    public string Service { get; set; }
    public double Price { get; set; }
    public string Length { get; set; }
    public string Description { get; set; }
    public double Discount { get; set; }
    public Bitmap Image_ID { get; set; }
    public string ImagePath { get; set; }
}
