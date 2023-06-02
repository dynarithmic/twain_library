package com.dtwain.test.displaydialogs;

import java.awt.event.ActionEvent;
import java.awt.event.ActionListener;
import java.awt.event.ComponentAdapter;
import java.awt.event.ComponentEvent;

import javax.swing.JButton;
import javax.swing.JDialog;
import javax.swing.JLabel;
import javax.swing.JTextField;


class GetSourceNameDialog extends JDialog {
	private JTextField textField;
	private String sourceName;
	
    public String getSelectedSource()
    {
        return sourceName;
    }
    /**
     * Create the frame
     */
    public GetSourceNameDialog() {
    	//super();
        super();
    	setModal(true);
    	addComponentListener(new ComponentAdapter() {
    		public void componentHidden(final ComponentEvent e) {
    			sourceName = textField.getText(); 
    		}
    	});
        setTitle("Select Source");
        getContentPane().setLayout(null);
        setBounds(100, 100, 288, 162);
        setResizable(false);

        final JLabel sourcesLabel = new JLabel();
        sourcesLabel.setText("Source Name:");
        sourcesLabel.setBounds(12, 12, 86, 30);
        getContentPane().add(sourcesLabel);

    	textField = new JTextField();
    	textField.setBounds(120, 15, 153, 26);
    	getContentPane().add(textField);

    	final JButton okButton = new JButton();
    	okButton.addActionListener(new ActionListener() {
    		public void actionPerformed(final ActionEvent e) {
    			sourceName = textField.getText(); 
    			setVisible(false);
    		}
    	});
    	okButton.setActionCommand("CloseDialog");
    	okButton.setText("Ok");
    	okButton.setBounds(105, 75, 80, 35);
    	getContentPane().add(okButton);
	}
}
