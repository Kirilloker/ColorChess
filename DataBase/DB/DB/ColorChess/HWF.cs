using FirstEF6App;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Data.Entity;

static class HWF
{
    static string GetNameUser(int userId)
    {
        using (ColorChessContext db = new ColorChessContext())
        {
            return db.Players.Where(b => b.Id == userId).ToList()[0].Name;
        }
    }


}