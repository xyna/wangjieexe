using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.Geometry;
using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.Runtime;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.GraphicsInterface;
using Autodesk.AutoCAD.LayerManager;
using Autodesk.AutoCAD.Windows;
using Autodesk.AutoCAD.Windows.ToolPalette;
using Autodesk.AutoCAD.Internal.Windows;
using Autodesk.AutoCAD.Internal.Forms;
using Autodesk.AutoCAD.BoundaryRepresentation;
using Autodesk.AutoCAD.Colors;

//[assembly: CommandClass(typeof(simpleTools.NEWBOX))]
//[assembly: CommandClass(typeof(simpleTools.ADDBLOCKDEF))]
[assembly: CommandClass(typeof(simpleTools.ADDENTTOMODELSPACE))]
namespace simpleTools
{
    //public class NEWBOX
    //{
    //    [CommandMethod("NewBox")]
    //    public Solid3d NewBox(double X, double Y, double Z) {
    //        Solid3d solid = new Solid3d();
    //        solid.CreateBox(X, Y, Z);
    //        return solid;
    //    }
    //}

    //public class ADDBLOCKDEF
    //{
    //    [CommandMethod("AddBlockDef")]
    //    public void AddBlockDef() {
    //        Database db = HostApplicationServices.WorkingDatabase;
    //        BlockTableRecord btr = new BlockTableRecord();
    //        btr.Name = "bimcad";
    //        Line line = new Line(Point3d.Origin, new Point3d(10, 15, 0));
    //        Circle circle = new Circle(Point3d.Origin, Vector3d.ZAxis, 10);
    //        btr.AppendEntity(line);
    //        btr.AppendEntity(circle);
    //        AddBlockTableRecord(btr, db);
    //    }

    //    public ObjectId AddBlockTableRecord(BlockTableRecord btr, Database db) {
    //        ObjectId id = new ObjectId();
    //        using (Transaction transaction = db.TransactionManager.StartTransaction()) {
    //            BlockTable bt = transaction.GetObject(db.BlockTableId, OpenMode.ForWrite) as BlockTable;
    //            id = bt.Add(btr);
    //            transaction.AddNewlyCreatedDBObject(btr, true);
    //            transaction.Commit();
    //        }
    //        return id;
    //    }
    //}
    public class ADDENTTOMODELSPACE
    {
        [CommandMethod("AddEntToModelSpace")]
        public void AddEntToModelSpace()
        {
            Database db = HostApplicationServices.WorkingDatabase;
            DBText txt = new DBText();
            txt.Position = new Point3d();
            txt.TextString = "BimCAD.org";
            ToModelSpace(txt, db);
        }

        /// <summary>
        ///  将一个图形对象加入到指定的Database的模型空间
        /// </summary>
        /// <param name="ent">实体对象</param>
        /// <param name="db">数据库</param>
        /// <returns>实体ObjectId</returns>
        public static ObjectId ToModelSpace(Entity ent, Database db)
        {
            ObjectId entId;
            using (Transaction trans = db.TransactionManager.StartTransaction())
            {
                BlockTable bt = (BlockTable)trans.GetObject(db.BlockTableId, OpenMode.ForRead);
                BlockTableRecord modelSpace = (BlockTableRecord)trans.GetObject(bt[BlockTableRecord.ModelSpace], OpenMode.ForWrite);
                entId = modelSpace.AppendEntity(ent);
                trans.AddNewlyCreatedDBObject(ent, true);
                trans.Commit();
            }
            return entId;
        }
    }


    //class ADDLAYER
    //{
    //    [CommandMethod("AddLayer")]
    //    public void AddLayer()
    //    {
    //        Database db = HostApplicationServices.WorkingDatabase;
    //        AddLayerTableRecord("BimCad.org", 1, db);
    //    }

    //    [CommandMethod("RemoveLayer")]
    //    public void RemoveLayer()
    //    {
    //        Database db = HostApplicationServices.WorkingDatabase;
    //        RemoveLayerTableRecord("BimCad.org", db);
    //    }

    //    /// <summary>
    //    /// 删除指定名字的图层
    //    /// </summary>
    //    /// <param name="LayerName">图层名</param>
    //    /// <param name="db">数据库</param>
    //    public static void RemoveLayerTableRecord(string layerName, Database db)
    //    {
    //        Editor ed = Application.DocumentManager.MdiActiveDocument.Editor;
    //        using (Transaction trans = db.TransactionManager.StartTransaction())
    //        {
    //            LayerTable lt = (LayerTable)trans.GetObject(db.LayerTableId, OpenMode.ForWrite);
    //            LayerTableRecord currentLayer = (LayerTableRecord)trans.GetObject(db.Clayer, OpenMode.ForRead);
    //            if (currentLayer.Name.ToLower() == layerName.ToLower())
    //                ed.WriteMessage("\n不能删除当前层");
    //            else
    //            {
    //                LayerTableRecord ltr = new LayerTableRecord();
    //                if (lt.Has(layerName))
    //                {
    //                    ltr = trans.GetObject(lt[layerName], OpenMode.ForWrite) as LayerTableRecord;
    //                    if (ltr.IsErased)
    //                        ed.WriteMessage("\n此层已经被删除");
    //                    else
    //                    {
    //                        ObjectIdCollection idCol = new ObjectIdCollection();
    //                        idCol.Add(ltr.ObjectId);
    //                        db.Purge(idCol);
    //                        if (idCol.Count == 0)
    //                            ed.WriteMessage("\n不能删除包含对象的图层");
    //                        else
    //                            ltr.Erase();
    //                    }
    //                }
    //                else
    //                    ed.WriteMessage("\n没有此图层");
    //            }
    //            trans.Commit();
    //        }
    //    }

    //    /// <summary>
    //    ///  建立指定名字，颜色的图层
    //    /// </summary>
    //    /// <param name="LayerName">图层名</param>
    //    /// <param name="ColorIndex">颜色索引</param>
    //    /// <param name="db">数据库</param>
    //    /// <returns>图层ObjectId</returns>
    //    public static ObjectId AddLayerTableRecord(string LayerName, short ColorIndex, Database db)
    //    {
    //        short colorIndex1 = (short)(ColorIndex % 256);//防止输入的颜色超出256
    //        using (Transaction trans = db.TransactionManager.StartTransaction())
    //        {
    //            LayerTable lt = (LayerTable)trans.GetObject(db.LayerTableId, OpenMode.ForWrite);
    //            ObjectId layerId = ObjectId.Null;
    //            if (lt.Has(LayerName) == false)
    //            {
    //                LayerTableRecord ltr = new LayerTableRecord();
    //                ltr.Name = LayerName;
    //                ltr.Color = Color.FromColorIndex(ColorMethod.ByColor, colorIndex1);
    //                layerId = lt.Add(ltr);
    //                trans.AddNewlyCreatedDBObject(ltr, true);
    //            }
    //            trans.Commit();
    //            return layerId;
    //        }
    //    }
    //}




}
