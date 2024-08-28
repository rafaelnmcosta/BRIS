namespace bris_API.Controllers
{
    public static class PoliticasDeAcesso
    {
        public const string VisualizacaoTotal = "Admin"; //visualiza tudo
        public const string VisualizacaoAgro = "Admin,GestorAgro,Visualizador"; //visualiza granjas, animais e usuarios da propria agroindustria
        public const string VisualizacaoGranja = "Admin,GestorAgro,GestorGranja,Visualizador"; // visualiza os animais e usuarios da propria granja
        public const string VisualizaAnimais = "Admin,GestorAgro,GestorGranja,Visualizador,Tecnico"; // visualiza os animais apenas (da granja)
        public const string GerenciaTotal = "Admin"; //gerencia tudo
        public const string GerenciaAgro = "Admin,GestorAgro"; // gerencia a agroindindustria atual: usuarios e granjas
        public const string GerenciaGranja = "Admin,GestorGranja"; // gerencia a granja atual: usuarios e animais
        public const string GerenciaAnimais = "Admin,GestorGranja,Tecnico"; // gerencia apenas animais 
        public const string TodosUsuarios = "Admin,GestorAgro,GestorGranja,Visualizador,Tecnico"; // Todos usuarios podem ver ou gerenciar isso
    }
}
