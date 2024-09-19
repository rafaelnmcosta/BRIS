import React from 'react';
import { Button } from 'antd';

const BotaoPrimario = ({ texto, icone }) => {
  return (
    <Button 
      type='primary' 
      shape='round' 
      icon={icone ? icone : null}>

      {texto}

    </Button>
  );
};

export default BotaoPrimario;
