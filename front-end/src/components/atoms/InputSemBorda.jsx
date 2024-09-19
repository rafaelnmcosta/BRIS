import React from 'react';
import { Input } from 'antd';

const InputSemBorda = ({ placeholder, icone }) => {
  return (
    <Input 
      variant='borderless' 
      placeholder={placeholder} 
      prefix={icone ? icone : null}
    />
  );
};

export default InputSemBorda;
