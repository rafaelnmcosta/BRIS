import React from 'react';
import { Button } from 'antd';

const BotaoPrimario = ({ texto, onClick }) => {
  return (
    <div>
      <Button type="primary" onClick={onClick} className="bg-green hover:!bg-green-dark text-white font-bold px-10 rounded-full h-10">
        {texto}
      </Button>
    </div>
  );
};

export default BotaoPrimario;
