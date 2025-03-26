import React from 'react';

const BotaoPrimario = ({ texto, onClick, type = 'button' }) => {
  return (
    <div>
      <button
        type={type}
        onClick={onClick}
        className="bg-green-dark hover:bg-green text-white text-l font-bold py-3 px-5 rounded-2xl transition-all duration-300 min-w-full"
      >
        {texto}
      </button>
    </div>
  );
};

export default BotaoPrimario;
