import React from 'react';
import { Button } from 'antd';

const BotaoMenu = ({ texto }) => {
  return (
    <Button type='primary'>
      {texto}
    </Button>
  );
};

export default BotaoMenu;
