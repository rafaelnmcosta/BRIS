import React from 'react';
import { Button } from 'antd';

const BotaoMenu = ({ texto, onClick }) => {
  return (
    <div>
      <Button type="primary" onClick={onClick} className="bg-green hover:!bg-green-dark text-white px-10 h-16">
        {texto}
      </Button>
    </div>
  );
};

export default BotaoMenu;
