import React from 'react';

const BotaoMenu = ({ texto, onClick }) => {
  return (
    <div>
      <button
        onClick={onClick}
        className="bg-green-dark hover:!bg-green text-white px-10 h-28 w-80 rounded-md duration-200"
      >
        {texto}
      </button>
    </div>
  );
};

export default BotaoMenu;
