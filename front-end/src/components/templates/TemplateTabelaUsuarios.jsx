import Tabela from '../organisms/Tabela';
import BotaoPrimario from '../atoms/BotaoPrimario';
import { useNavigate } from 'react-router-dom';

const TemplateTabela = ({ tipo, lista }) => {
  const navigate = useNavigate();

  return (
    <div className="container mx-auto pt-8 h-fit">
      {/* Header com título e botões */}
      <div className="flex justify-between items-center mb-6">
        <h1 className="text-2xl font-bold text-green-dark">Lista de {tipo}s</h1>
        
        <div className="flex gap-4 w-fit">
          <BotaoPrimario 
            texto="Cadastrar novo" 
            onClick={() => navigate('/usuarios/cadastrar')}
          />
          
          <BotaoPrimario 
            texto="Listar inativos" 
            onClick={() => navigate('/usuarios/inativos')}
          />
          
        </div>
      </div>

      {/* Tabela */}
      <Tabela tipo={tipo} lista={lista} />
    </div>
  );
};

export default TemplateTabela;