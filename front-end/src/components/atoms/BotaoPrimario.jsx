import React from 'react';
import { Button } from 'antd';

const BotaoPrimario = ({ texto, type = "submit" }) => {
  return (
    <div>
      <Button
        type={type}  // Garantindo que o tipo seja 'submit' aqui
        className="bg-green hover:!bg-green-dark text-white font-bold px-10 rounded-full h-10"
      >
        {texto}
      </Button>
    </div>
  );
};

export default BotaoPrimario;
