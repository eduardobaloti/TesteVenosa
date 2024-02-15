// Exemplo: api/exemplo/controllers/exemplo.js

module.exports = {
  async find(ctx) {
    // Sua lógica de busca padrão aqui...

    // Adicione a lógica para gerar um número aleatório
    const numeroAleatorio = Math.floor(Math.random() * 100) + 1;

    // Inclua o número aleatório na resposta
    ctx.send({
      data: {
        numeroAleatorio: numeroAleatorio,
      },
    });
  },
};