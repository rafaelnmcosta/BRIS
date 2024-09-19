import React from 'react';
import { Alert } from 'antd';

const MensagemBox = ({ mensagem, texto, tipo }) => {
    return (
        <Alert
            message={mensagem}
            description={texto}
            type={tipo}
            showIcon
            closable
        />
    );
};

export default MensagemBox;
