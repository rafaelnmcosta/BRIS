import React from 'react';

const BotaoPrimario = ({ texto, onClick, type = 'button' }) => {
  return (
    <div>
      <button
        type={type}
        onClick={onClick}
        className="bg-green-dark hover:bg-green text-white font-bold px-10 rounded-full h-10 transition-all duration-300"
      >
        {texto}
      </button>
    </div>
  );
};

export default BotaoPrimario;
