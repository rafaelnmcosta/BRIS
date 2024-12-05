import React from 'react';
import { useNotification } from '../../services/NotificationContext';

const BotaoOff = ({ texto }) => {
    const abrirNotificacao = useNotification();

    return (
        <div>
            <button
                onClick={() =>
                    abrirNotificacao('error', 'Erro de acesso', 'Esta função ainda não está disponível')
                }
                className="bg-gray-400 text-white px-10 h-28 w-80 rounded-md cursor-not-allowed"
            >
                {texto}
            </button>
        </div>
    );
};

export default BotaoOff;
