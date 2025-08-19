using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
//using ProductStoreMVC.Data;
using ProductStoreMVC.Models;

namespace MidExamDemo.Controllers;

public class ProductsController : Controller
{
    private readonly AppDbContext _db;

    public ProductsController(AppDbContext db) => _db = db;

    // GET: /Products
    public async Task<IActionResult> Index(int? categoryId)
    {
        var query = _db.Products.Include(p => p.Category).AsQueryable();
        if (categoryId.HasValue)
            query = query.Where(p => p.CategoryId == categoryId);

        ViewBag.Categories = new SelectList(await _db.Categories.ToListAsync(), "Id", "Name", categoryId);
        return View(await query.OrderByDescending(p => p.Id).ToListAsync());
    }

    // GET: /Products/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id is null) return NotFound();
        var product = await _db.Products.Include(p => p.Category).FirstOrDefaultAsync(p => p.Id == id);
        if (product is null) return NotFound();
        return View(product);
    }

    // GET: /Products/Create
    public async Task<IActionResult> Create()
    {
        await LoadCategoriesAsync();
        return View(new Product());
    }

    // POST: /Products/Create
    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Product model)
    {
        if (!ModelState.IsValid)
        {
            await LoadCategoriesAsync(model.CategoryId);
            return View(model);
        }

        _db.Add(model);
        await _db.SaveChangesAsync();
        TempData["ok"] = "Thêm sản phẩm thành công!";
        return RedirectToAction(nameof(Index));
    }

    // GET: /Products/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id is null) return NotFound();
        var product = await _db.Products.FindAsync(id);
        if (product is null) return NotFound();

        await LoadCategoriesAsync(product.CategoryId);
        return View(product);
    }

    // POST: /Products/Edit/5
    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, Product model)
    {
        if (id != model.Id) return BadRequest();

        if (!ModelState.IsValid)
        {
            await LoadCategoriesAsync(model.CategoryId);
            return View(model);
        }

        try
        {
            _db.Update(model);
            await _db.SaveChangesAsync();
            TempData["ok"] = "Cập nhật thành công!";
            return RedirectToAction(nameof(Index));
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!await _db.Products.AnyAsync(p => p.Id == id)) return NotFound();
            throw;
        }
    }

    // GET: /Products/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id is null) return NotFound();
        var product = await _db.Products.Include(p => p.Category).FirstOrDefaultAsync(m => m.Id == id);
        if (product is null) return NotFound();
        return View(product);
    }

    // POST: /Products/Delete/5
    [HttpPost, ActionName("Delete"), ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var product = await _db.Products.FindAsync(id);
        if (product is not null)
        {
            _db.Products.Remove(product);
            await _db.SaveChangesAsync();
            TempData["ok"] = "Đã xóa!";
        }
        return RedirectToAction(nameof(Index));
    }

    private async Task LoadCategoriesAsync(int? selectedId = null)
    {
        ViewBag.CategoryId = new SelectList(await _db.Categories.ToListAsync(), "Id", "Name", selectedId);
    }
}
