import React from 'react';
import { Input } from 'antd';

const InputSenha = ({ placeholder }) => {
  return (
    <Input.Password placeholder={placeholder} />
  );
};

export default InputSenha;