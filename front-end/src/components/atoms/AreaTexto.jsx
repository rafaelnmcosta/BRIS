import React from 'react';
import { Input } from 'antd';
const { TextArea } = Input;

const AreaTexto = ({ placeholder, maxLength }) => {
  return (
    <TextArea maxLength={maxLength} placeholder={placeholder}/>
  );
};

export default AreaTexto;
