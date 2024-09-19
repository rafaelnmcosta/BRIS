import React from 'react';
import { Input } from 'antd';

const InputPadrao = ({ placeholder, icone }) => {
  return (
    <Input 
      placeholder={placeholder} 
      prefix={icone ? icone : null}
    />
  );
};

export default InputPadrao;
