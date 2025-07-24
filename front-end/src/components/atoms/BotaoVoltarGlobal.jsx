import { useNavigate, useLocation } from 'react-router-dom';
import { ArrowLeftOutlined } from '@ant-design/icons';

const BotaoVoltarGlobal = () => {
  const navigate = useNavigate();
  const location = useLocation();

  // Páginas onde o botão NÃO vai aparecer (não esquece de acrescentar outras dps se precisar bocó)
  const rotasSemBotao = ['/', '/login', '/vinculos', '/home'];

  if (rotasSemBotao.includes(location.pathname)) {
    return null;
  }

  const handleVoltar = () => {
    if (window.history.length > 2) {
      navigate(-1);
    } else {
      navigate('/home'); // fallback
    }
  };

  return (
    <div className="fixed bottom-4 left-4 z-50">
      <button
        onClick={handleVoltar}
        className="bg-green hover:bg-green-dark text-white text-l font-bold py-3 px-5 rounded-full shadow-lg flex items-center gap-2 transition-all duration-300"
      >
        <ArrowLeftOutlined className="text-white text-base" />
        Voltar
      </button>
    </div>
  );
};

export default BotaoVoltarGlobal;
