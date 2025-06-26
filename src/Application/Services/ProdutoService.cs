using Core.Data.Repositories;
using Core.Domain.Entities;
using Core.Utils;
using Core.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using System.Security.Claims;

namespace Core.Services;

public class ProdutoService : IProdutoService
{
    private readonly ICategoriaRepository _categoriaRepository;
    private readonly Guid _currentUserId;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IProdutoRepository _produtoRepository;
    private readonly Upload _upload;

    public ProdutoService(IProdutoRepository produtoRepository, IHostEnvironment hostingEnvironment,
        IHttpContextAccessor httpContextAccessor, ICategoriaRepository categoriaRepository, Upload upload)
    {
        _produtoRepository = produtoRepository;
        _httpContextAccessor = httpContextAccessor;
        _categoriaRepository = categoriaRepository;
        _upload = upload;
        _currentUserId = GetCurrentUserId();
    }

    public async Task<List<Produto>> GetAsync(CancellationToken cancellationToken)
    {
        return await _produtoRepository.GetAsync(cancellationToken);
    }

    public async Task<List<Produto>> GetByVendedorId(CancellationToken cancellationToken)
    {
        return await _produtoRepository.GetByVendedorIdAsync(_currentUserId, cancellationToken);
    }

    public async Task<Produto> FindAsync(Guid id, CancellationToken cancellationToken)
    {
        return await _produtoRepository.FindAsync(id, cancellationToken);
    }

    public async Task<List<Produto>> GetByCategoriaIdAsync(Guid categoriaId, CancellationToken cancellationToken)
    {
        return await _produtoRepository.GetByCategoriaIdAsync(categoriaId, cancellationToken);
    }

    public async Task<Guid> CreateAsync(CreateProdutoViewModel createProdutoViewModel,
        CancellationToken cancellationToken, string? path = null)
    {
        var isValidCategory =
            await _categoriaRepository.IsValidCategoryAsync(createProdutoViewModel.CategoriaId, cancellationToken);
        if (!isValidCategory)
            throw new KeyNotFoundException("Categoria inválida");

        var produto = new Produto
        {
            Nome = createProdutoViewModel.Nome,
            Descricao = createProdutoViewModel.Descricao,
            Preco = createProdutoViewModel.Preco,
            Estoque = createProdutoViewModel.Estoque,
            CategoriaId = createProdutoViewModel.CategoriaId,
            VendedorId = _currentUserId
        };
        if (createProdutoViewModel.ImagemUpload != null)
            await _upload.AddImageAsync(produto, createProdutoViewModel.ImagemUpload, path, cancellationToken);

        return await _produtoRepository.CreateAsync(produto, cancellationToken);
    }

    public async Task UpdateAsync(UpdateProdutoViewModel updateProdutoViewModel, CancellationToken cancellationToken)
    {
        var produto = await _produtoRepository.FindAsync(updateProdutoViewModel.Id, cancellationToken);
        if (produto is null) throw new KeyNotFoundException("Produto não encontrado");

        if (!IsUserOwner(produto))
            throw new UnauthorizedAccessException("Ação não permitida.");

        produto.Nome = updateProdutoViewModel.Nome ?? produto.Nome;
        produto.Descricao = updateProdutoViewModel.Descricao ?? produto.Descricao;
        produto.Preco = updateProdutoViewModel.Preco ?? produto.Preco;
        produto.Estoque = updateProdutoViewModel.Estoque ?? produto.Estoque;
        produto.Ativo = updateProdutoViewModel.Ativo;

        await _produtoRepository.UpdateAsync(produto, cancellationToken);
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken)
    {
        var produto = await _produtoRepository.FindAsync(id, cancellationToken);
        if (!IsUserOwner(produto))
            throw new UnauthorizedAccessException("Ação não permitida.");

        await _produtoRepository.DeleteAsync(id, cancellationToken);
    }

    private bool IsUserOwner(Produto? produto)
    {
        return produto != null && produto.VendedorId == _currentUserId;
    }

    private Guid GetCurrentUserId()
    {
        var user = _httpContextAccessor.HttpContext?.User;
        if (user?.Identity?.IsAuthenticated != true)
            return Guid.Empty;

        var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrWhiteSpace(userId))
            throw new Exception("Usuário não encontrado");

        return Guid.Parse(userId);
    }
}

public interface IProdutoService
{
    Task<List<Produto>> GetAsync(CancellationToken cancellationToken);

    Task<List<Produto>> GetByVendedorId(CancellationToken cancellationToken);

    Task<Produto> FindAsync(Guid id, CancellationToken cancellationToken);

    Task<List<Produto>> GetByCategoriaIdAsync(Guid categoriaId, CancellationToken cancellationToken);

    Task<Guid> CreateAsync(CreateProdutoViewModel createProdutoViewModel, CancellationToken cancellationToken,
        string? path = null);

    Task UpdateAsync(UpdateProdutoViewModel updateProdutoViewModel, CancellationToken cancellationToken);

    Task DeleteAsync(Guid id, CancellationToken cancellationToken);
}